using LinkyAppBackend.Api.Exceptions.Service;
using LinkyAppBackend.Api.Handlers.Interfaces;
using LinkyAppBackend.Api.Models.Dtos.Auth;
using LinkyAppBackend.Api.Models.Entities;
using LinkyAppBackend.Api.Models.Entities.Master;
using LinkyAppBackend.Api.Models.Options;
using LinkyAppBackend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LinkyAppBackend.Api.Services;

public class AuthService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IJwtHandler jwtHandler,
    AppDbContext dbContext,
    IOptions<JwtOptions> jwtSettings)
    : IAuthService
{
    private readonly JwtOptions _jwtOptions = jwtSettings.Value;

    public async Task<TokenDto> SignUp(RegisterDto dto)
    {
        var user = await RegisterUser(dto);
        return await GetTokens(user);
    }

    public async Task<TokenDto> SignIn(LoginDto dto)
    {
        var user = await userManager.FindByNameAsync(dto.Username) ??
                   await userManager.FindByEmailAsync(dto.Username);

        if (user is null)
            throw new UnauthorizedException("Invalid credentials");

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

        if (signInResult.IsLockedOut) throw new UnauthorizedException("Account is locked out");
        if (signInResult.RequiresTwoFactor) throw new UnauthorizedException("Multi factor authentication is required");
        if (signInResult.IsNotAllowed) throw new UnauthorizedException("Not allowed");
        if (!signInResult.Succeeded) throw new UnauthorizedException("Invalid credentials");

        return await GetTokens(user);
    }

    public async Task<TokenDto> GoogleSignIn(ExternalAuthDto dto)
    {
        var payload = await jwtHandler.VerifyGoogleToken(dto);

        var info = new UserLoginInfo(dto.Provider, payload.Subject, dto.Provider);
        var user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        if (user == null)
        {
            user = await userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = await RegisterUser(new RegisterDto
                {
                    Username = payload.Email,
                    Email = payload.Email
                }, true);
                await userManager.AddLoginAsync(user, info);
            }
            else
            {
                await userManager.AddLoginAsync(user, info);
            }
        }

        return await GetTokens(user);
    }

    public async Task<TokenDto> Refresh(string refreshToken)
    {
        var token = dbContext.RefreshTokens
            .Include(t => t.User).SingleOrDefault(t => t.Token == refreshToken);

        if (token == null)
            throw new UnauthorizedException("Invalid refresh token");

        if (token.Expiration < DateTime.UtcNow)
            throw new UnauthorizedException("Refresh token expired");

        var user = token.User;
        user.RefreshTokens.Remove(token);
        await userManager.UpdateAsync(user);

        return await GetTokens(user);
    }

    #region Private Methods

    // //TODO refactor to use server name parameter and email template
    // private async Task SendConfirmationEmail(string email, AppUser user)
    // {
    //     var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
    //     var confirmationLink = $"http://localhost/confirm-email?UserId={user.Id}&Token={token}";
    //     await emailSender.SendEmailAsync(email, "Confirm Your Email",
    //         $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>;.");
    // }

    private async Task<AppUser> RegisterUser(RegisterDto dto, bool isExternal = false)
    {
        var user = new AppUser { UserName = dto.Username, Email = dto.Email };
        var result = isExternal
            ? await userManager.CreateAsync(user)
            : await userManager.CreateAsync(user, dto.Password);

        if (result.Errors.Any())
            throw new BadRequestException(result.Errors.First().Description);

        if (!result.Succeeded)
        {
            throw new BadRequestException("Failed to create user account.");
        }

        // await SendConfirmationEmail(user.Email, user);

        return user;
    }

    private async Task<TokenDto> GetTokens(AppUser user)
    {
        var token = jwtHandler.GenerateJwtToken(user);
        var refreshToken = jwtHandler.GenerateRefreshToken();

        var dbToken = new RefreshToken
        {
            Token = refreshToken,
            Expiration = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshExpirationTime),
            User = user
        };

        user.RefreshTokens.Add(dbToken);
        await userManager.UpdateAsync(user);

        return new TokenDto { AccessToken = token, RefreshToken = refreshToken };
    }

    #endregion
}