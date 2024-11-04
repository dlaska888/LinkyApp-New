using Google.Apis.Auth;
using LinkyAppBackend.Api.Models.Dtos.Auth;
using LinkyAppBackend.Api.Models.Entities.Master;

namespace LinkyAppBackend.Api.Handlers.Interfaces;

public interface IJwtHandler
{
    string GenerateJwtToken(AppUser user);
    string GenerateRefreshToken();
    Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto dto);
}