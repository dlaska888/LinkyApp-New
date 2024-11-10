using FluentEmail.Core;
using LinkyAppBackend.Api.Models.Templates;
using LinkyAppBackend.Api.Services.Interfaces;

namespace LinkyAppBackend.Api.Services;

public class EmailService(IFluentEmail email, IWebHostEnvironment env) : IEmailService
{
    private readonly string _templatePath = Path.Combine(env.ContentRootPath, "Views/Templates");

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        await email
            .To(toEmail)
            .Subject(subject)
            .Body(body)
            .SendAsync();
    }
    
    public async Task SendEmailConfirmationAsync(string toEmail, ConfirmEmailModel model)
    {
        await email
            .To(toEmail)
            .Subject("Confirm Your Email Address")
            .UsingTemplateFromFile(GetTemplatePath("ConfirmEmail"), model)
            .SendAsync();
    }

    public async Task SendPasswordResetAsync(string toEmail, ResetPasswordModel model)
    {
        await email
            .To(toEmail)
            .Subject("Reset Your Password")
            .UsingTemplateFromFile(GetTemplatePath("ResetPassword"), model)
            .SendAsync();
    }

    public async Task SendEmailChangeConfirmationAsync(string toEmail, ChangeEmailModel model)
    {
        await email
            .To(toEmail)
            .Subject("Confirm Your New Email Address")
            .UsingTemplateFromFile(GetTemplatePath("EmailChange"), model)
            .SendAsync();
    }
    
    private string GetTemplatePath(string templateName)
    {
        return Path.Combine(_templatePath, $"{templateName}.cshtml");
    }
}