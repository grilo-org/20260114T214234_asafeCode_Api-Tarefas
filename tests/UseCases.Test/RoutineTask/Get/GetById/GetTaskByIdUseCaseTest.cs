using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories.RoutineTask;
using Shouldly;
using TarefasCrud.Application.UseCases.RoutineTask.GetById;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace UseCases.Test.RoutineTask.Get.GetById;

public class GetTaskByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var(user, _) = UserBuilder.Build();
        var task = TaskBuilder.Build(user);
        task.Id = 1;
        var useCase = CreateUseCase(user, task);

        var result = await useCase.Execute(task.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBeGreaterThan(0);
        result.Title.ShouldBe(task.Title);
    }

    [Fact]
    public async Task Error_Recipe_NotFound()
    {
        var(user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => { await useCase.Execute(taskId: 1000); };

        var exception = await act.ShouldThrowAsync<NotFoundException>();
        
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.TASK_NOT_FOUND);
    }

    private static GetTaskByIdUseCase CreateUseCase(
        TarefasCrud.Domain.Entities.User user,
        TaskEntity? task = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new TaskReadOnlyRepositoryBuilder().GetTaskById(user, task).Build();

        return new GetTaskByIdUseCase(repository, loggedUser);
    }
}