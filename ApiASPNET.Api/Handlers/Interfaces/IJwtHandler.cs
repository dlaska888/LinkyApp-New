using ApiASPNET.Api.Models.Dtos.Auth;
using ApiASPNET.Api.Models.Entities.Master;
using Google.Apis.Auth;

namespace ApiASPNET.Api.Handlers.Interfaces;

public interface IJwtHandler
{
    string GenerateJwtToken(AppUser user);
    string GenerateRefreshToken();
    Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto dto);
}