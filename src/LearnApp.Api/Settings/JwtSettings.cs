using System.ComponentModel.DataAnnotations;

namespace LearnApp.Api.Settings;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    [Required] public string Issuer { get; set; } = "";
    [Required]public string Audience { get; set; } = "";
    [Range(1, 1440)] public int ExpiryMinutes { get; set; } = 60;

    [Required, MinLength(32)] public string SigningKey { get; set; } = "";
}
