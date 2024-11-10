using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LinkyAppBackend.Api.Models.Entities;
using LinkyAppBackend.Api.Providers;
using LinkyAppBackend.Api.Models.Dtos.Account;
using LinkyAppBackend.Api.Models.Entities.Master;
using LinkyAppBackend.Api.Services;
using LinkyAppBackend.Api.Services.Interfaces;

namespace LinkyAppBackend.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class Account(IAccountService service) : ControllerBase
{
    [HttpGet]
    public async Task<GetAccountDto> Get()
    {
        return await service.GetAsync();
    }

    [HttpPost("update-username")]
    public async Task UpdateUsername(UpdateUsernameDto dto)
    {
        await service.UpdateUsernameAsync(dto);
    }

    [HttpPost("update-password")]
    public async Task UpdatePassword(UpdatePasswordDto dto)
    {
        await service.UpdatePasswordAsync(dto);
    }

    [HttpPost("upload-profile-picture")]
    public async Task UploadProfilePicture(IFormFile file)
    {
        await service.UploadProfilePictureAsync(file);
    }

    [HttpDelete("delete-profile-picture")]
    public async Task DeleteProfilePicture()
    {
        await service.DeleteProfilePictureAsync();
    }

    [AllowAnonymous]
    [HttpPost("request-email-confirmation")]
    public async Task RequestEmailConfirmation(RequestEmailConfirmationDto dto)
    {
        await service.RequestEmailConfirmationAsync(dto);
    }

    [AllowAnonymous]
    [HttpPost("confirm-email")]
    public async Task ConfirmEmail(ConfirmEmailDto dto)
    {
        await service.ConfirmEmailAsync(dto);
    }

    [HttpPost("request-email-change")]
    public async Task RequestEmailChange(RequestEmailChangeDto dto)
    {
        await service.RequestEmailChangeAsync(dto);
    }

    [HttpPost("confirm-email-change")]
    public async Task ConfirmEmailChange(ConfirmEmailChangeDto dto)
    {
        await service.ConfirmEmailChangeAsync(dto);
    }

    [AllowAnonymous]
    [HttpPost("request-password-reset")]
    public async Task RequestPasswordReset(RequestPasswordResetDto dto)
    {
        await service.RequestPasswordResetAsync(dto);
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task ResetPassword(ResetPasswordDto dto)
    {
        await service.ResetPasswordAsync(dto);
    }
}