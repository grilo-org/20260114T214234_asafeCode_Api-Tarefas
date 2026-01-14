using Moq;
using TarefasCrud.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories.User;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository = new();
    
    public void ExistsActiveUserWithEmail(string email)
    {
        _repository.Setup(repository => repository.ExistsActiveUserWithEmail(email)).ReturnsAsync(true);
    }    
    public void GetUserByEmail(TarefasCrud.Domain.Entities.User user)
    {
        _repository.Setup(repository => repository.GetUserByEmail(user.Email)).ReturnsAsync(user);
    }
    
    public IUserReadOnlyRepository Build() => _repository.Object;
}