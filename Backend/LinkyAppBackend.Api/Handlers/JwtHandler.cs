﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using LinkyAppBackend.Api.Exceptions;
using LinkyAppBackend.Api.Exceptions.Service;
using LinkyAppBackend.Api.Handlers.Interfaces;
using LinkyAppBackend.Api.Models.Dtos.Auth;
using LinkyAppBackend.Api.Models.Entities.Master;
using LinkyAppBackend.Api.Models.Options;
using LinkyAppBackend.Api.Models.Options.SocialAuth;

namespace LinkyAppBackend.Api.Handlers;

public class JwtHandler(IOptions<JwtOptions> jwtOptions, IOptions<GoogleOptions> googleOptions) : IJwtHandler
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly GoogleOptions _googleOptions = googleOptions.Value;

    public string GenerateJwtToken(AppUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationTime);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
            },
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
    }

    public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto dto)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new List<string> { _googleOptions.ClientId }
        };

        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken, settings);
            return payload;
        }
        catch (Exception e)
        {
            throw new UnauthorizedException("Invalid Google token");
        }
    }
}