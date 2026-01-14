using Moq;
using TarefasCrud.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories.User;

public static class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build() => new Mock<IUserWriteOnlyRepository>().Object;
}