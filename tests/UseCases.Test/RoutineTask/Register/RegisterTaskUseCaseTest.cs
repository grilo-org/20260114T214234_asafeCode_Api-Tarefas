using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Providers;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.RoutineTask;
using CommonTestUtilities.Requests;
using CommonTestUtilities.ValueObjects;
using Shouldly;
using TarefasCrud.Application.UseCases.RoutineTask.Register;
using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace UseCases.Test.RoutineTask.Register;

public class RegisterTaskUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestTaskJsonBuilder.Build();
        var useCase = CreateUseCase(user);
        var result = await useCase.Execute(request);

        result.ShouldNotBeNull(); result.Id.ShouldBeGreaterThanOrEqualTo(0);
        result.Title.ShouldBe(request.Title);
    }
    
    [Fact]
    public async Task Error_Title_Empty()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestTaskJsonBuilder.Build();
        request.Title = string.Empty;
        
        var useCase = CreateUseCase(user);
        
        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();
        
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.TASK_TITLE_EMPTY);
    }    
    
    [Fact]
    public async Task Error_Description_Exceeds_Character_Limit()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestTaskJsonBuilder.Build(descriptionChar: 200);
        
        var useCase = CreateUseCase(user);
        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();
        
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.DESCRIPTION_EXCEEDS_LIMIT_CHARACTERS);
    }   

    private static RegisterTaskUseCase CreateUseCase(TarefasCrud.Domain.Entities.User? user = null)
    {
        var loggerUser = LoggedUserBuilder.Build(user!);
        var writeRepository = TaskWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var dateProvider = new DateProviderBuilder().UseCaseToday(TarefasCrudTestsConstants.DateForTests).Build();
        
        return new RegisterTaskUseCase(loggerUser, writeRepository, unitOfWork, dateProvider);
    }
}