using TarefasCrud.Domain.Entities;

namespace TarefasCrud.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    public Task<User> User();
}