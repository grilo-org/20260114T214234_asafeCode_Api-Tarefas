using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Providers;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.RoutineTask;
using CommonTestUtilities.Requests;
using CommonTestUtilities.ValueObjects;
using Shouldly;
using TarefasCrud.Application.UseCases.RoutineTask.UpdateTask;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace UseCases.Test.RoutineTask.Update.RoutineTask;

public class UpdateTaskUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var recipe = TaskBuilder.Build(user);
        var request = RequestTaskJsonBuilder.Build();

        var useCase = CreateUseCase(user, recipe);

        var act = async () => await useCase.Execute(recipe.Id, request);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Task_NotFound()
    {
        var(user, _) = UserBuilder.Build();

        var request = RequestTaskJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(taskId: 1000, request);

        var exception = await act.ShouldThrowAsync<NotFoundException>();
        
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.TASK_NOT_FOUND);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var(user, _) = UserBuilder.Build();
        var recipe = TaskBuilder.Build(user);
        var request = RequestTaskJsonBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(user, recipe);

        var act = async () => await useCase.Execute(recipe.Id, request);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();
        
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.TASK_TITLE_EMPTY);
    }

    private static UpdateTaskUseCase CreateUseCase(
        TarefasCrud.Domain.Entities.User user,
        TaskEntity? task = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var repository = new TaskUpdateOnlyRepositoryBuilder().GetById(user, task).Build();
        var dateProvider = new DateProviderBuilder().UseCaseToday(TarefasCrudTestsConstants.DateForTests).Build();

        return new UpdateTaskUseCase(repository, loggedUser, unitOfWork, dateProvider);
    }
}