using LinkyAppBackend.Api.Exceptions.Service;
using LinkyAppBackend.Api.Handlers.Interfaces;
using LinkyAppBackend.Api.Models.Dtos.Auth;
using LinkyAppBackend.Api.Models.Entities;
using LinkyAppBackend.Api.Models.Entities.Master;
using LinkyAppBackend.Api.Models.Options;
using LinkyAppBackend.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LinkyAppBackend.Api.Services;

public class AuthService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IJwtHandler jwtHandler,
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
        var user = userManager.Users.SingleOrDefault(u =>
            u.RefreshToken == refreshToken && u.RefreshTokenExp > DateTime.UtcNow);

        if (user == null)
        {
            throw new UnauthorizedException("Invalid refresh token");
        }

        if (user.RefreshTokenExp < DateTime.UtcNow)
        {
            throw new UnauthorizedException("Refresh token expired");
        }

        return await GetTokens(user);
    }

    public async Task<bool> ConfirmEmail(string userId, string token)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new BadRequestException("User not found");
        }

        var result = await userManager.ConfirmEmailAsync(user, token);

        if (result.Errors.Any())
            throw new BadRequestException(result.Errors.First().Description);

        if (!result.Succeeded)
        {
            throw new BadRequestException("Failed to create user account.");
        }

        return result.Succeeded;
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

        user.RefreshToken = refreshToken;
        user.RefreshTokenExp = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtOptions.RefreshExpirationTime));
        await userManager.UpdateAsync(user);

        return new TokenDto { AccessToken = token, RefreshToken = refreshToken };
    }

    #endregion
}