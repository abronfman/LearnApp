using System.Security.Claims;
using System.Text;
using LearnApp.Api.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace LearnApp.Api.Auth;

public interface ITokenService 
{
    (string Token, DateTime ExpiresAtUtc) CreateToken(DemoUser user);
}

public class TokenService(IOptions<JwtSettings> options) : ITokenService
{
    private readonly JwtSettings _jwt = options.Value;

    public (string Token, DateTime ExpiresAtUtc) CreateToken(DemoUser user)
    {
        var expires = DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwt.Issuer,
            Audience = _jwt.Audience,
            Expires = expires,

            Subject = new ClaimsIdentity(
            [
                new Claim("sub", user.Id),
                new Claim("name", user.Name),
                new Claim("email", user.Email),
                new Claim("role", user.Role)
            ]),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SigningKey)),
                SecurityAlgorithms.HmacSha256)
        };
        return (new JsonWebTokenHandler().CreateToken(descriptor), expires);
    }
}
