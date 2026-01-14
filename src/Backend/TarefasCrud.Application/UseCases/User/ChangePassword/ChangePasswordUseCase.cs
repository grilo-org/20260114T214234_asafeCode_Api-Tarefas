using FluentValidation.Results;
using TarefasCrud.Application.SharedValidators;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Repositories;
using TarefasCrud.Domain.Repositories.User;
using TarefasCrud.Domain.Security.Criptography;
using TarefasCrud.Domain.Services.LoggedUser;
using TarefasCrud.Exceptions;

namespace TarefasCrud.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUserUpdateOnlyRepository  _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    
    public ChangePasswordUseCase(
        IPasswordEncripter passwordEncripter, 
        IUserUpdateOnlyRepository repository, 
        ILoggedUser loggedUser, 
        IUnitOfWork unitOfWork)
    {
        _passwordEncripter = passwordEncripter;
        _repository = repository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.User();
        Validate(request, loggedUser.Password);
        
        var user = await _repository.GetUserById(loggedUser.Id);
        user.Password = _passwordEncripter.Encrypt(request.NewPassword);
        
        _repository.Update(user);
        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, string currentPassword)
    {
        var validator = new ChangePasswordValidator();
        var result = validator.Validate(request);
        
        if (result.IsValid.IsFalse())
            HandleValidationResult.ThrowError(result);
        
        if (_passwordEncripter.IsValid(request.Password, currentPassword))
            return;
        
        result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
        HandleValidationResult.ThrowError(result);
    }
}