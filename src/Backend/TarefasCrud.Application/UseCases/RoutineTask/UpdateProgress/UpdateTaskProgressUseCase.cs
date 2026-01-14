using TarefasCrud.Application.SharedValidators;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Enums;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Providers;
using TarefasCrud.Domain.Repositories;
using TarefasCrud.Domain.Repositories.Tasks;
using TarefasCrud.Domain.Services.LoggedUser;
using TarefasCrud.Domain.ValueObjects;
using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace TarefasCrud.Application.UseCases.RoutineTask.UpdateProgress;

public class UpdateTaskProgressUseCase : IUpdateTaskProgressUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITaskUpdateOnlyRepository _updateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateProvider _dateProvider;
    public UpdateTaskProgressUseCase(ILoggedUser loggedUser, 
        ITaskUpdateOnlyRepository updateRepository, 
        IUnitOfWork unitOfWork, 
        IDateProvider dateProvider)
    {
        _loggedUser = loggedUser;
        _updateRepository = updateRepository;
        _unitOfWork = unitOfWork;
        _dateProvider = dateProvider;
    }
    public async Task Execute(long taskId, ProgressOperation operation)
    {
        var loggedUser = await _loggedUser.User();
        var task = await _updateRepository.GetById(loggedUser, taskId);
        
        if (task is null)
            throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);

        Validate(operation, task);
        
        if (operation == ProgressOperation.Decrement && task.IsCompleted)
            task.IsCompleted = false;
        
        task.Progress += operation.ToInt();
        
        if (task.Progress == task.WeeklyGoal)
            task.IsCompleted = true;
        
        _updateRepository.Update(task);
        await _unitOfWork.Commit();
    }

    private void Validate(ProgressOperation operation, TaskEntity task)
    {
        if (task.WeekOfMonth.Equals(_dateProvider.UseCaseDate.GetMonthWeek()).IsFalse())
            throw new ConflictException(ResourceMessagesException.ONLY_MODIFY_PROGRESS_CURRENT_WEEK);

        if (operation == ProgressOperation.Increment && task.IsCompleted)
            throw new ConflictException(ResourceMessagesException.NOT_INCREMENT_COMPLETED_TASK);
        
        if (operation == ProgressOperation.Decrement && task.Progress == TarefasCrudRuleConstants.INITIAL_PROGRESS)
            throw new ConflictException(ResourceMessagesException.NOT_DECREMENT_INITIAL_PROGRESS_TASK);
    }
}