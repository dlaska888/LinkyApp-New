﻿using LinkyAppBackend.Api.Models.Dtos.Auth;

namespace LinkyAppBackend.Api.Services.Interfaces;

public interface IAuthService
{
    Task<TokenDto> SignUp(RegisterDto dto);
    Task<TokenDto> SignIn(LoginDto dto);
    Task<TokenDto> GoogleSignIn(ExternalAuthDto dto);
    Task<TokenDto> Refresh(string refreshToken);
}