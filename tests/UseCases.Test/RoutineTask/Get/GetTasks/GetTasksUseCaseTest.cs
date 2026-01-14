using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories.RoutineTask;
using Shouldly;
using TarefasCrud.Application.UseCases.RoutineTask.GetTasks;
using TarefasCrud.Domain.Dtos;
using TarefasCrud.Domain.Entities;

namespace UseCases.Test.RoutineTask.Get.GetTasks;

public class GetTasksUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var tasks = TaskBuilder.Collection(user);
        var filters = new FilterTasksDto();
        var useCase = CreateUseCase(user, tasks, filters);

        var result = await useCase.Execute(filters);

        result.ShouldNotBeNull();
        result.Tasks.Count.ShouldBeGreaterThan(0);
        result.Tasks.Select(x => x.Id).ShouldBeUnique();
        foreach (var task in result.Tasks)
        {
            task.Id.ShouldBeGreaterThan(0);
            task.Title.ShouldNotBeNullOrWhiteSpace();
            task.WeeklyGoal.ShouldBeGreaterThan(0);
            task.Category.ShouldNotBeNullOrWhiteSpace();
        }
    }
    private static GetTasksUseCase CreateUseCase(
        TarefasCrud.Domain.Entities.User user,
        IList<TaskEntity> tasks, FilterTasksDto filters)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new TaskReadOnlyRepositoryBuilder().GetTasks(user, tasks, filters).Build();

        return new GetTasksUseCase(repository, loggedUser);
    }
}