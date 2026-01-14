using Moq;
using TarefasCrud.Domain.Repositories;
using TarefasCrud.Infrastructure.DataAccess;

namespace CommonTestUtilities.Repositories;

public class UnitOfWorkBuilder
{
    public static IUnitOfWork Build() => new Mock<IUnitOfWork>().Object;
}