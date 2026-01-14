using Moq;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Repositories.Tasks;

namespace CommonTestUtilities.Repositories.RoutineTask;

public class TaskUpdateOnlyRepositoryBuilder
{
    private readonly Mock<ITaskUpdateOnlyRepository> _repository;

    public TaskUpdateOnlyRepositoryBuilder() => _repository = new Mock<ITaskUpdateOnlyRepository>();

    public TaskUpdateOnlyRepositoryBuilder GetById(TarefasCrud.Domain.Entities.User user, TaskEntity? task)
    {
        if (task is not null)
            _repository.Setup(repository => repository.GetById(user, task.Id)).ReturnsAsync(task);

        return this;
    }

    public ITaskUpdateOnlyRepository Build() => _repository.Object;
}