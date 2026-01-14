using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Requests;
using Shouldly;
using TarefasCrud.Application.UseCases.User.ChangePassword;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, password) = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = password;
        
        var useCase = CreateUseCase(user);
        var act = async () => await useCase.Execute(request);
        await act.ShouldNotThrowAsync();
    }    
    
    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        var (user, password) = UserBuilder.Build();

        var request = new RequestChangePasswordJson
        {
            NewPassword = string.Empty,
            Password = password
        };
        
        var useCase = CreateUseCase(user);
        
        var act = async () => await useCase.Execute(request);
        
        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();
        
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.PASSWORD_EMPTY);
    }    
    
    [Fact]
    public async Task Error_CurrentPassword_Different()
    {
        var (user, _) = UserBuilder.Build();
        
        var request = RequestChangePasswordJsonBuilder.Build();
        
        var useCase = CreateUseCase(user);
        
        var act = async () => await useCase.Execute(request);
        
        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();
        
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD);
    } 
    
    
    private static ChangePasswordUseCase CreateUseCase(TarefasCrud.Domain.Entities.User? user = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user!);
        var repository = new UserUpdateOnlyRepositoryBuilder().GetById(user!).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        
        return new ChangePasswordUseCase(passwordEncripter, repository, loggedUser, unitOfWork);
    }
}