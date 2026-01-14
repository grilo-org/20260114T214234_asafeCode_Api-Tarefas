using Mapster;
using TarefasCrud.Communication.Responses;
using TarefasCrud.Domain.Repositories.Tasks;
using TarefasCrud.Domain.Services.LoggedUser;
using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace TarefasCrud.Application.UseCases.RoutineTask.GetById;

public class GetTaskByIdUseCase  : IGetTaskByIdUseCase
{
    private readonly ITaskReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    public GetTaskByIdUseCase(ITaskReadOnlyRepository repository, 
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseTaskJson> Execute(long taskId)
    {
        var loggedUser = await _loggedUser.User();
        var task = await _repository.GetById(loggedUser, taskId);
        
        if (task is null)
            throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
        var response = task.Adapt<ResponseTaskJson>();
        return response;
    }
}