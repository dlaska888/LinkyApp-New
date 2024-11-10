using System.ComponentModel.DataAnnotations;

namespace LinkyAppBackend.Api.Models.Dtos.Account;

public class RequestEmailChangeDto
{
    [EmailAddress] public string NewEmail { get; set; } = null!;
}