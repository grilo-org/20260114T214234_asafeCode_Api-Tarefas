using TarefasCrud.Communication.Requests;
using TarefasCrud.Communication.Responses;

namespace TarefasCrud.Application.UseCases.Token.RefreshToken;

public interface IUseRefreshTokenUseCase
{
    public Task<ResponseTokensJson> Execute(RequestNewTokenJson request);
}