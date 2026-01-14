using Moq;
using OpenAI.Responses;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Repositories.Token;

namespace CommonTestUtilities.Repositories.Tokens;

public class TokenRepositoryBuilder
{
    private readonly Mock<ITokenRepository> _repository = new();
    
    public TokenRepositoryBuilder Get(RefreshToken? refreshToken)
    {
        if(refreshToken is not null)
            _repository.Setup(repository => repository.Get(refreshToken.Value)).ReturnsAsync(refreshToken);

        return this;
    }

    public ITokenRepository Build() => _repository.Object;
}