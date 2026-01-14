using Mapster;
using TarefasCrud.Application.SharedValidators;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Communication.Responses;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Providers;
using TarefasCrud.Domain.Repositories;
using TarefasCrud.Domain.Repositories.Tasks;
using TarefasCrud.Domain.Services.LoggedUser;
using TarefasCrud.Domain.ValueObjects;

namespace TarefasCrud.Application.UseCases.RoutineTask.Register;

public class RegisterTaskUseCase : IRegisterTaskUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITaskWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateProvider _dateProvider;
    
    public RegisterTaskUseCase(ILoggedUser loggedUser, 
        ITaskWriteOnlyRepository repository, 
        IUnitOfWork unitOfWork, 
        IDateProvider dateProvider)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _dateProvider = dateProvider;
    }
    public async Task<ResponseRegisteredTaskJson> Execute(RequestTaskJson request)
    {
        Validate(request);
        var loggedUser = await _loggedUser.User();
        var task = request.Adapt<TaskEntity>();
        task.UserId = loggedUser.Id;
        task.WeekOfMonth = task.StartDate.GetMonthWeek();
        task.Progress = TarefasCrudRuleConstants.INITIAL_PROGRESS;

        await _repository.Add(task);
        await _unitOfWork.Commit();

        return new ResponseRegisteredTaskJson
        {
            Id = task.Id,
            Title = task.Title,
        };
    }

    private void Validate(RequestTaskJson request)
    {
        var date = _dateProvider.UseCaseDate;
        var validator = new TaskValidator(date);
        var result = validator.Validate(request);
        
        if (result.IsValid)
            return;
        
        HandleValidationResult.ThrowError(result);
    }
}