using FluentValidation.Results;
using TarefasCrud.Application.SharedValidators;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Repositories;
using TarefasCrud.Domain.Repositories.User;
using TarefasCrud.Domain.Services.LoggedUser;
using TarefasCrud.Exceptions;

namespace TarefasCrud.Application.UseCases.User.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly ILoggedUser  _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserUseCase(IUserUpdateOnlyRepository repository, 
        IUserReadOnlyRepository readOnlyRepository, 
        ILoggedUser loggedUser, 
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _readOnlyRepository = readOnlyRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser = await _loggedUser.User();
        await Validate(request, loggedUser.Email);
        
        var user = await _repository.GetUserById(loggedUser.Id);
        user.Name = request.Name;
        user.Email = request.Email;
        
        _repository.Update(user);
        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request,  string currentEmail)
    {
        var validator = new UpdateUserValidator();
        var result = await validator.ValidateAsync(request);
        
        if (result.IsValid.IsFalse())
            HandleValidationResult.ThrowError(result);
        
        if (currentEmail.Equals(request.Email).IsFalse())
        {
            var emailExists = await _readOnlyRepository.ExistsActiveUserWithEmail(request.Email);
            if (emailExists)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
                HandleValidationResult.ThrowError(result);
            }
        }
    }
}