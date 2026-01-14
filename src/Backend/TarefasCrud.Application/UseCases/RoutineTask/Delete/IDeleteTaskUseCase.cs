namespace TarefasCrud.Application.UseCases.RoutineTask.Delete;

public interface IDeleteTaskUseCase
{
    public Task Execute(long taskId);
}