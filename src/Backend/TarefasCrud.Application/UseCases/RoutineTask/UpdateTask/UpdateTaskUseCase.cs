using TarefasCrud.Application.SharedValidators;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Providers;
using TarefasCrud.Domain.Repositories;
using TarefasCrud.Domain.Repositories.Tasks;
using TarefasCrud.Domain.Services.LoggedUser;
using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace TarefasCrud.Application.UseCases.RoutineTask.UpdateTask;

public class UpdateTaskUseCase :  IUpdateTaskUseCase
{
    private readonly ITaskUpdateOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateProvider _dateProvider;
    
    public UpdateTaskUseCase(ITaskUpdateOnlyRepository repository, 
        ILoggedUser loggedUser, 
        IUnitOfWork unitOfWork, 
        IDateProvider dateProvider)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _dateProvider = dateProvider;
    }
    public async Task Execute(long taskId, RequestTaskJson request)
    {
        var loggedUser = await _loggedUser.User();
        
        var task = await _repository.GetById(loggedUser, taskId);
        if (task is null)
            throw new NotFoundException(ResourceMessagesException.TASK_NOT_FOUND);
        
        Validate(request, task);

        task.Title = request.Title;
        
        if (request.Description.NotEmpty())
            task.Description = request.Description;
        
        task.StartDate = request.StartDate;
        task.Category = request.Category;
        task.WeeklyGoal = request.WeeklyGoal;
        
        _repository.Update(task);
        await _unitOfWork.Commit();
    }
    private void Validate(RequestTaskJson request, TaskEntity task)
    {
        var date = _dateProvider.UseCaseDate;
        var validator = new TaskValidator(date, task);
        var result = validator.Validate(request);
        
        if (result.IsValid)
            return;
        
        HandleValidationResult.ThrowError(result);
    }
}