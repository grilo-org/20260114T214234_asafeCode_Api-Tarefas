using TarefasCrud.Domain.Entities;

namespace TarefasCrud.Domain.Repositories.Tasks;

public interface ITaskUpdateOnlyRepository
{
    public Task<TaskEntity?> GetById(Entities.User user, long taskId);
    public void Update(TaskEntity task);
}