using System.ComponentModel;

namespace ApiASPNET.Api.Models.Dtos.Auth;

public class LoginDto
{
    [DefaultValue("username")] public string Username { get; set; } = null!;
    [DefaultValue("!QAZ3wsx1234")] public string Password { get; set; } = null!;
}