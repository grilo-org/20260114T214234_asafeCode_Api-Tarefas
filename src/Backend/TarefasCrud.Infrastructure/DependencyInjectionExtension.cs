using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarefasCrud.Domain.Providers;
using TarefasCrud.Domain.Repositories;
using TarefasCrud.Domain.Repositories.Tasks;
using TarefasCrud.Domain.Repositories.Token;
using TarefasCrud.Domain.Repositories.User;
using TarefasCrud.Domain.Security.Criptography;
using TarefasCrud.Domain.Security.Tokens;
using TarefasCrud.Domain.Services.LoggedUser;
using TarefasCrud.Infrastructure.DataAccess;
using TarefasCrud.Infrastructure.DataAccess.Repositories;
using TarefasCrud.Infrastructure.Extensions;
using TarefasCrud.Infrastructure.Providers;
using TarefasCrud.Infrastructure.Security.Criptography;
using TarefasCrud.Infrastructure.Security.Tokens.Access.Generator;
using TarefasCrud.Infrastructure.Security.Tokens.Access.Validator;
using TarefasCrud.Infrastructure.Security.Tokens.Refresh;
using TarefasCrud.Infrastructure.Services;
using TarefasCrud.Infrastructure.Settings;

namespace TarefasCrud.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddTokens(services, configuration);
        AddLoggedUser(services);
        AddPasswordEncripter(services);
        AddDbContext_SqlServer(services, configuration);
        AddFluentMigrator_SqlServer(services, configuration);
        AddProviders(services);
    }

    private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        services.AddDbContext<TarefasCrudDbContext>(
            dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
            });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<ITaskWriteOnlyRepository, TasksRepository>();
        services.AddScoped<ITaskReadOnlyRepository, TasksRepository>();
        services.AddScoped<ITaskUpdateOnlyRepository, TasksRepository>();
    }
    private static void AddProviders(IServiceCollection services)
    {
        services.AddScoped<IDateProvider, DateProvider>();
    }

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        // var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        // var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");
        
        services.Configure<JwtSettings>(options =>
            configuration.GetSection("Settings:Jwt").Bind(options)
        );

        services.AddScoped<IAccessTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAccessTokenValidator, JwtTokenValidator>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
    }
    
    private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();
        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("TarefasCrud.Infrastructure")).For.All();
        });
    }
    
    private static void AddLoggedUser(IServiceCollection services)
    { 
       services.AddScoped<ILoggedUser, LoggedUser>();
    }
    
    private static void AddPasswordEncripter(this IServiceCollection services)
    {
        services.AddScoped<IPasswordEncripter, BcryptEncripter>();
    }
}   