using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TarefasCrud.Infrastructure.Security.Tokens.Access;

public class JwtTokenHandler
{
    protected JwtTokenHandler() {}
    protected static SymmetricSecurityKey SecurityKey(string signingKey)
    {
        var bytes = Encoding.UTF8.GetBytes(signingKey);
        return new SymmetricSecurityKey(bytes);
    }
}