using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Providers;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.RoutineTask;
using CommonTestUtilities.ValueObjects;
using Shouldly;
using TarefasCrud.Application.UseCases.RoutineTask.UpdateProgress;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Enums;
using TarefasCrud.Exceptions;
using TarefasCrud.Exceptions.ExceptionsBase;

namespace UseCases.Test.RoutineTask.Update.Progress;

public class UpdateTaskProgressUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var task = TaskBuilder.Build(user);
        var useCase = CreateUseCase(user, task);
        
        var act = async () => await useCase.Execute(task.Id, ProgressOperation.Increment);
        
        await act.ShouldNotThrowAsync();
    }    
    [Fact]
    public async Task Success_Decrement()
    {
        var (user, _) = UserBuilder.Build();
        var task = TaskBuilder.Build(user);
        var useCase = CreateUseCase(user, task);
        
        var act1 = async () => await useCase.Execute(task.Id, ProgressOperation.Increment);
        var act2 = async () => await useCase.Execute(task.Id, ProgressOperation.Decrement);
        
        await act1.ShouldNotThrowAsync();
        await act2.ShouldNotThrowAsync();
    }    
    
    [Fact]
    public async Task Error_Task_NotFound()
    {
        var (user, _) = UserBuilder.Build();
        var task = TaskBuilder.Build(user);
        var useCase = CreateUseCase(user, task);
        
        var act = async () => await useCase.Execute(taskId: 1000, ProgressOperation.Increment);
        
        var exception = await act.ShouldThrowAsync<NotFoundException>();
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.TASK_NOT_FOUND);
    }    
    
    [Fact]
    public async Task Error_Increment_Completed_Task()
    {
        var (user, _) = UserBuilder.Build();
        var task = TaskBuilder.Build(user);
        task.IsCompleted = true;
        var useCase = CreateUseCase(user, task);
        
        var act = async () => await useCase.Execute(task.Id, ProgressOperation.Increment);
        
        var exception = await act.ShouldThrowAsync<ConflictException>();
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.NOT_INCREMENT_COMPLETED_TASK);
    }       
    [Fact]
    public async Task Error_Handle_Invalid_Week_Task()
    {
        var (user, _) = UserBuilder.Build();
        var task = TaskBuilder.Build(user);
        task.WeekOfMonth = 1;  
        var useCase = CreateUseCase(user, task);
        
        var act = async () => await useCase.Execute(task.Id, ProgressOperation.Increment);
        
        var exception = await act.ShouldThrowAsync<ConflictException>();
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.ONLY_MODIFY_PROGRESS_CURRENT_WEEK);
    }    
    
    [Fact]
    public async Task Error_Decrement_Task_With_Initial_Progress()
    {
        var (user, _) = UserBuilder.Build();
        var task = TaskBuilder.Build(user);
        var useCase = CreateUseCase(user, task);
        
        var act = async () => await useCase.Execute(task.Id, ProgressOperation.Decrement);
        
        var exception = await act.ShouldThrowAsync<ConflictException>();
        exception.GetErrorMessages().Count.ShouldBe(1);
        exception.GetErrorMessages().ShouldContain(ResourceMessagesException.NOT_DECREMENT_INITIAL_PROGRESS_TASK);
    }
    
    private static UpdateTaskProgressUseCase CreateUseCase(
        TarefasCrud.Domain.Entities.User user,
        TaskEntity? task = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var repository = new TaskUpdateOnlyRepositoryBuilder().GetById(user, task).Build();
        var dateProvider = new DateProviderBuilder().UseCaseToday(TarefasCrudTestsConstants.DateForTests).Build();


        return new UpdateTaskProgressUseCase(loggedUser, repository, unitOfWork, dateProvider);
    }
}