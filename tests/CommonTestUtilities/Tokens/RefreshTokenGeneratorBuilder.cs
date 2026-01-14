using TarefasCrud.Domain.Security.Tokens;
using TarefasCrud.Infrastructure.Security.Tokens.Refresh;

namespace CommonTestUtilities.Tokens;

public static class RefreshTokenGeneratorBuilder
{
    public static IRefreshTokenGenerator Build() => new RefreshTokenGenerator();
}