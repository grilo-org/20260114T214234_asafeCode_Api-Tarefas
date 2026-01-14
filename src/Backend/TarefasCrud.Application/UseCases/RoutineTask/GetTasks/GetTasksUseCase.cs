using Mapster;
using TarefasCrud.Communication.Responses;
using TarefasCrud.Domain.Dtos;
using TarefasCrud.Domain.Repositories.Tasks;
using TarefasCrud.Domain.Services.LoggedUser;

namespace TarefasCrud.Application.UseCases.RoutineTask.GetTasks;

public class GetTasksUseCase : IGetTasksUseCase
{
    private readonly ITaskReadOnlyRepository _readRepository;
    private readonly ILoggedUser _loggedUser;
    public GetTasksUseCase(ITaskReadOnlyRepository readRepository, 
        ILoggedUser loggedUser)
    {
        _readRepository = readRepository;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseTasksJson> Execute(FilterTasksDto filters)
    {
        var loggedUser = await _loggedUser.User();
        var tasks = await _readRepository.GetTasks(loggedUser, filters);

        return new ResponseTasksJson
        {
            Tasks = tasks.Adapt<IList<ResponseShortTaskJson>>()
        };
    }
}