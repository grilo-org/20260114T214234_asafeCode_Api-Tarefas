using Moq;
using TarefasCrud.Domain.Dtos;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Repositories.Tasks;

namespace CommonTestUtilities.Repositories.RoutineTask;

public class TaskReadOnlyRepositoryBuilder
{
    private readonly Mock<ITaskReadOnlyRepository> _repository = new();

    public TaskReadOnlyRepositoryBuilder GetTaskById(TarefasCrud.Domain.Entities.User user, TaskEntity? task)
    {
        if (task is not null)
            _repository.Setup(repository => repository.GetById(user, task.Id)).ReturnsAsync(task);
        
        return this;
    }    
    public TaskReadOnlyRepositoryBuilder GetTasks(TarefasCrud.Domain.Entities.User user, IList<TaskEntity> tasks, FilterTasksDto filter)
    {
        _repository.Setup(repository => repository.GetTasks(user, filter)).ReturnsAsync(tasks);
        return this;
    }
    
    public ITaskReadOnlyRepository Build() => _repository.Object;
}