using Moq;
using TarefasCrud.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories.User;

public class UserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUserUpdateOnlyRepository> _repository = new();
    
    public UserUpdateOnlyRepositoryBuilder GetById(TarefasCrud.Domain.Entities.User user)
    {
        _repository.Setup(repository => repository.GetUserById(user.Id)).ReturnsAsync(user);
        return this;
    }    
    
    public IUserUpdateOnlyRepository Build() => _repository.Object;
}