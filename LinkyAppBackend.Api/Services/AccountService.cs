﻿using LinkyAppBackend.Api.Models.Dtos.Account;
using LinkyAppBackend.Api.Models.Entities;
using LinkyAppBackend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.Extensions.Options;
using LinkyAppBackend.Api.Exceptions.Service;
using LinkyAppBackend.Api.Models.Entities.Master;
using LinkyAppBackend.Api.Providers;
using Microsoft.EntityFrameworkCore;
using TaggyAppBackend.Api.Models.Options;
using TaggyAppBackend.Api.Repos.Interfaces;
using File = LinkyAppBackend.Api.Models.Entities.Master.File;

namespace LinkyAppBackend.Api.Services;

public class AccountService(
    AppDbContext dbContext,
    UserManager<AppUser> userManager,
    IEmailService emailService,
    IAuthContextProvider auth,
    IOptions<AzureBlobOptions> azureBlobOptions,
    IBlobRepo blobRepo,
    IMapper mapper
) : IAccountService
{
    public async Task<GetAccountDto> GetAsync()
    {
        var userId = auth.GetUserId();
        var user = (await dbContext.Users
            .Include(u => u.ProfilePhoto)
            .FirstOrDefaultAsync(u => u.Id == userId))!;

        var profilePictureUrl = user.ProfilePhoto != null
            ? await blobRepo.GetBlobDownloadPath(
                user.ProfilePhoto.TrustedName,
                azureBlobOptions.Value.Container,
                user.ProfilePhoto.UntrustedName)
            : null;

        var mapped = mapper.Map<GetAccountDto>(user);
        mapped.ProfilePictureUrl = profilePictureUrl;

        return mapped;
    }

    public async Task UpdateUsernameAsync(UpdateUsernameDto dto)
    {
        var user = (await userManager.FindByIdAsync(auth.GetUserId()))!;
        user.UserName = dto.Username;
        var result = await userManager.UpdateAsync(user);
        HandleIdentityErrors(result);
    }

    public async Task UpdatePasswordAsync(UpdatePasswordDto dto)
    {
        var user = (await userManager.FindByIdAsync(auth.GetUserId()))!;
        var result = await userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        HandleIdentityErrors(result);
    }

    public async Task UploadProfilePictureAsync(IFormFile file)
    {
        var user = (await userManager.FindByIdAsync(auth.GetUserId()))!;

        var dbFile = new File();

        var trustedName = $"{dbFile.Id}.{Path.GetExtension(file.FileName)}";
        var blobInfo =
            await blobRepo.UploadBlob(trustedName, azureBlobOptions.Value.Container, file.OpenReadStream());

        dbFile.TrustedName = trustedName;
        dbFile.UntrustedName = file.FileName;
        dbFile.ContentType = blobInfo.ContentType;
        dbFile.Size = blobInfo.Size;

        user.ProfilePhoto = dbFile;
        var result = await userManager.UpdateAsync(user);
        HandleIdentityErrors(result);
    }

    public async Task DeleteProfilePictureAsync()
    {
        var userId = auth.GetUserId();
        var user = (await dbContext.Users
            .Include(u => u.ProfilePhoto)
            .FirstOrDefaultAsync(u => u.Id == userId))!;

        if (user.ProfilePhoto == null)
            throw new BadRequestException("Profile picture not found");

        await blobRepo.DeleteBlob(user.ProfilePhoto.TrustedName, azureBlobOptions.Value.Container);
        var result = await userManager.UpdateAsync(user);
        HandleIdentityErrors(result);
    }

    public async Task RequestEmailConfirmationAsync()
    {
        var user = (await userManager.FindByIdAsync(auth.GetUserId()))!;

        if (user.EmailConfirmed)
            throw new BadRequestException("Email is already confirmed");

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = _appSettings.Value.FrontendUrl + "/confirm-email?token=" + token;

        await emailService.SendEmailConfirmationAsync(user.Email, confirmationLink);
    }

    public async Task ConfirmEmailAsync(string token)
    {
        var user = (await userManager.FindByIdAsync(auth.GetUserId()))!;
        var result = await userManager.ConfirmEmailAsync(user, token);
        HandleIdentityErrors(result, "Failed to confirm email.");
    }

    public async Task RequestEmailChangeAsync(RequestEmailChangeDto dto)
    {
        var user = (await userManager.FindByIdAsync(auth.GetUserId()))!;
        var token = await userManager.GenerateChangeEmailTokenAsync(user, dto.NewEmail);
        await emailService.SendEmailChangeConfirmationAsync(dto.NewEmail, token);
    }

    public async Task ConfirmEmailChangeAsync(ConfirmEmailChangeDto dto)
    {
        var user = (await userManager.FindByIdAsync(auth.GetUserId()))!;
        var result = await userManager.ChangeEmailAsync(user, dto.NewEmail, dto.Token);
        HandleIdentityErrors(result, "Failed to change email.");
    }

    public async Task RequestPasswordResetAsync(RequestPasswordResetDto dto)
    {
        var user = (await userManager.FindByEmailAsync(dto.Email))!;

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = _appSettings.Value.FrontendUrl + "/reset-password?token=" + token;

        await emailService.SendPasswordResetAsync(user.Email, resetLink);
    }

    public async Task ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = (await userManager.FindByEmailAsync(dto.Email))!;
        var result = await userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
        HandleIdentityErrors(result, "Failed to reset password.");
    }

    private void HandleIdentityErrors(IdentityResult result, string messageOnFail = "Request failed")
    {
        if (!result.Succeeded)
            throw new BadRequestException(
                result.Errors.Any()
                    ? result.Errors.First().Description
                    : messageOnFail
            );
    }
}