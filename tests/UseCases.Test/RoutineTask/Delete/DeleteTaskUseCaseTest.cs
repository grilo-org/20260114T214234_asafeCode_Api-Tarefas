using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.RoutineTask;
using Shouldly;
using TarefasCrud.Application.UseCases.RoutineTask.Delete;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace UseCases.Test.RoutineTask.Delete;

public class DeleteTaskUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var task = TaskBuilder.Build(user);

        var useCase = CreateUseCase(user, task);

        var act = async () => { await useCase.Execute(task.Id); };

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Task_NotFound()
    {
        var (user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => { await useCase.Execute(taskId: 1000); };

        var exception = await act.ShouldThrowAsync<NotFoundException>();
        
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.TASK_NOT_FOUND);
    }
    private static DeleteTaskUseCase CreateUseCase(TarefasCrud.Domain.Entities.User user, TaskEntity? task = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var readRepository = new TaskReadOnlyRepositoryBuilder().GetTaskById(user, task!).Build();
        var writeRepository = TaskWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        
        return new DeleteTaskUseCase(loggedUser, readRepository, writeRepository, unitOfWork);
    }
}