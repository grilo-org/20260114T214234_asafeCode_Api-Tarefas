using TarefasCrud.Domain.Entities;

namespace TarefasCrud.Domain.Repositories.Tasks;

public interface ITaskWriteOnlyRepository
{
    public Task Add(TaskEntity task);
    public Task Delete(long taskId);
}