using LinkyAppBackend.Api.Models.Templates;

namespace LinkyAppBackend.Api.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
    Task SendEmailConfirmationAsync(string toEmail, ConfirmEmailModel model);
    Task SendPasswordResetAsync(string toEmail, ResetPasswordModel model);
    Task SendEmailChangeConfirmationAsync(string toEmail, ChangeEmailModel model);
}