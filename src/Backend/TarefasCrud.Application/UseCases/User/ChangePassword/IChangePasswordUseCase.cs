using TarefasCrud.Communication.Requests;

namespace TarefasCrud.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    public Task Execute(RequestChangePasswordJson request);
}