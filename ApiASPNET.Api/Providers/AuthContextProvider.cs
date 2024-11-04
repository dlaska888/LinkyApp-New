using System.Security.Claims;

namespace ApiASPNET.Api.Providers;

// Methods not nullable because they are called only when user is authenticated
public class AuthContextProvider(IHttpContextAccessor httpContextAccessor) : IAuthContextProvider
{
    public string GetUserId()
    {
        return httpContextAccessor.HttpContext?.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value!;
    }

    public string GetUserEmail()
    {
        return httpContextAccessor.HttpContext?.User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value!;
    }

    public string GetUsername()
    {
        return httpContextAccessor.HttpContext?.User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value!;
    }
}