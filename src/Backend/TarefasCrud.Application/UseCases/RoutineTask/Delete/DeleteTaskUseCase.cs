using TarefasCrud.Domain.Repositories;
using TarefasCrud.Domain.Repositories.Tasks;
using TarefasCrud.Domain.Services.LoggedUser;
using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace TarefasCrud.Application.UseCases.RoutineTask.Delete;

public class DeleteTaskUseCase : IDeleteTaskUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITaskReadOnlyRepository _readRepository;
    private readonly ITaskWriteOnlyRepository _writeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaskUseCase(ILoggedUser loggedUser, 
        ITaskReadOnlyRepository readRepository, 
        ITaskWriteOnlyRepository writeRepository, 
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long taskId)
    {
        var loggedUser = await _loggedUser.User();
        var task = await _readRepository.GetById(loggedUser, taskId);

        if (task is null)
            throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
        await _writeRepository.Delete(taskId);
        await _unitOfWork.Commit();
    }
}