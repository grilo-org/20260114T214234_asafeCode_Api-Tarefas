using TarefasCrud.Communication.Responses;
using TarefasCrud.Domain.Dtos;

namespace TarefasCrud.Application.UseCases.RoutineTask.GetTasks;

public interface IGetTasksUseCase
{
    public Task<ResponseTasksJson> Execute(FilterTasksDto filters);
}