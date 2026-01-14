using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TarefasCrud.Domain.Providers;
using TarefasCrud.Domain.Security.Tokens;
using TarefasCrud.Infrastructure.Settings;

namespace TarefasCrud.Infrastructure.Security.Tokens.Access.Generator;

public class JwtTokenGenerator : JwtTokenHandler, IAccessTokenGenerator
{
    private readonly JwtSettings _settings;
    public JwtTokenGenerator(IOptions<JwtSettings> options) => _settings = options.Value;

    public string Generate(Guid userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, userId.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_settings.ExpirationTimeMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(_settings.SigningKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}