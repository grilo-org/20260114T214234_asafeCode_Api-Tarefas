using TarefasCrud.Communication.Responses;

namespace TarefasCrud.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfileJson> Execute();
}