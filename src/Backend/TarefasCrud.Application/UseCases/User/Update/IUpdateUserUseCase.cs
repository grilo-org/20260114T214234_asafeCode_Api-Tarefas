using TarefasCrud.Communication.Requests;

namespace TarefasCrud.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    public Task Execute(RequestUpdateUserJson request);
}