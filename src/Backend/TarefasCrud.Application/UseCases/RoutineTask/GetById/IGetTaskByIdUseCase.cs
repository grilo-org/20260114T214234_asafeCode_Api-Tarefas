using TarefasCrud.Communication.Responses;

namespace TarefasCrud.Application.UseCases.RoutineTask.GetById;

public interface IGetTaskByIdUseCase
{
    public Task<ResponseTaskJson> Execute(long taskId);
}