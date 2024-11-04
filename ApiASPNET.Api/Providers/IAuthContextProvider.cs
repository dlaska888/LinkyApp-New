namespace ApiASPNET.Api.Providers;

public interface IAuthContextProvider
{
    string GetUserId();
    string GetUserEmail();
    string GetUsername();
}