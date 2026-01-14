using Microsoft.Extensions.DependencyInjection;
using TarefasCrud.Application.Services;
using TarefasCrud.Application.UseCases.Login;
using TarefasCrud.Application.UseCases.RoutineTask.Delete;
using TarefasCrud.Application.UseCases.RoutineTask.GetById;
using TarefasCrud.Application.UseCases.RoutineTask.GetTasks;
using TarefasCrud.Application.UseCases.RoutineTask.Register;
using TarefasCrud.Application.UseCases.RoutineTask.UpdateProgress;
using TarefasCrud.Application.UseCases.RoutineTask.UpdateTask;
using TarefasCrud.Application.UseCases.Token.RefreshToken;
using TarefasCrud.Application.UseCases.User.ChangePassword;
using TarefasCrud.Application.UseCases.User.Profile;
using TarefasCrud.Application.UseCases.User.Register;
using TarefasCrud.Application.UseCases.User.Update;

namespace TarefasCrud.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddUseCases(services);
        AddMapper();
    }
    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        
        services.AddScoped<IUseRefreshTokenUseCase, UseRefreshTokenUseCase>();
        
        services.AddScoped<IRegisterTaskUseCase, RegisterTaskUseCase>();
        services.AddScoped<IGetTaskByIdUseCase, GetTaskByIdUseCase>();
        services.AddScoped<IGetTasksUseCase, GetTasksUseCase>();
        services.AddScoped<IUpdateTaskProgressUseCase, UpdateTaskProgressUseCase>();
        services.AddScoped<IUpdateTaskUseCase, UpdateTaskUseCase>();
        services.AddScoped<IDeleteTaskUseCase, DeleteTaskUseCase>();
        
    }
    private static void AddMapper()
    {
        MapConfigurations.Configure();
    }
}