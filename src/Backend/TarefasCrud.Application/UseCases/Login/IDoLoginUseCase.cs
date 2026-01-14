using TarefasCrud.Communication.Requests;
using TarefasCrud.Communication.Responses;

namespace TarefasCrud.Application.UseCases.Login;

public interface IDoLoginUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}