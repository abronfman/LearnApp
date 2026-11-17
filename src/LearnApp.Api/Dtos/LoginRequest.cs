using System.ComponentModel.DataAnnotations;

namespace LearnApp.Api.Dtos;

public class LoginRequest
{
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required] public string Password { get; set; } = "";
}

public record LoginResponse(string AccessToken, DateTime ExpriesAtUtc, IReadOnlyList<string> Roles);

public record MeResponse(string? Name, string? Email, IReadOnlyList<string> Roles);
