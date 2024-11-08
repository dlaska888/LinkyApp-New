﻿namespace LinkyAppBackend.Api.Models.Options;

public class MailOptions
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FromEmail { get; set; } = null!;
    public string FromName { get; set; } = null!;
}