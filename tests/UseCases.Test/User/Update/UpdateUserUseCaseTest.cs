using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.User;
using CommonTestUtilities.Requests;
using Shouldly;
using TarefasCrud.Application.UseCases.User.Update;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Update;

public class UpdateUserUseCaseTest
{
     [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        
        var request = RequestUpdateUserJsonBuilder.Build();
        
        var useCase = CreateUseCase(user);
        
        var act = async () => await useCase.Execute(request);
        
        await act.ShouldNotThrowAsync();
        
        user.Name.ShouldBe(request.Name);
        user.Email.ShouldBe(request.Email);
    }    
    
    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        var useCase = CreateUseCase(user, request.Email);
        var act = async () => await useCase.Execute(request);
        var exception =  await act.ShouldThrowAsync<ErrorOnValidationException>();
        
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.EMAIL_ALREADY_REGISTERED);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        var useCase = CreateUseCase(user);
        var act = async () => await useCase.Execute(request);
        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();
        
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.NAME_EMPTY);
    }
    
    
    private static UpdateUserUseCase CreateUseCase(TarefasCrud.Domain.Entities.User? user = null, string? email = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user!);
        var readRepository =  new UserReadOnlyRepositoryBuilder();
        var updateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user!).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if (email.NotEmpty())
        {
            readRepository.ExistsActiveUserWithEmail(email);
        }
        
        return new UpdateUserUseCase(updateRepository, readRepository.Build(), loggedUser, unitOfWork);
    }
}