using CommonTestUtilities.Entities;
using CommonTestUtilities.Providers;
using CommonTestUtilities.ValueObjects;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TarefasCrud.API;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Providers;
using TarefasCrud.Infrastructure.DataAccess;
using TarefasCrud.Infrastructure.Providers;
using Testcontainers.MsSql;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private static MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Strong_password_123!")
        .Build();
    
    private string _connectionString = string.Empty;
    private string _password = string.Empty;
    private TarefasCrud.Domain.Entities.User _user = null!;
    private TaskEntity _task = null!;
    private RefreshToken _refreshToken = null!;
    public async Task InitializeAsync()
    {
        await InitializeContainer();
        var dbContext = Services.GetRequiredService<TarefasCrudDbContext>();
        await StartDatabase(dbContext);
    }
    public new async Task DisposeAsync() => await Task.CompletedTask;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureAppConfiguration(opt =>
            {
                opt.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {
                        "ConnectionStrings:ConnectionSqlServer",  _connectionString
                    }
                });
            })
            
            .ConfigureServices(services =>
            {
                services.RemoveAll<IDateProvider>();
                services.AddScoped<IDateProvider, FixedFakeDateForTests>();
                var descriptor = services.SingleOrDefault(desc =>
                    desc.ServiceType == typeof(DbContextOptions<TarefasCrudDbContext>));
                
                if (descriptor is not null)
                    services.Remove(descriptor);
                
                services.AddDbContext<TarefasCrudDbContext>(opt =>
                    opt.UseSqlServer(_connectionString));
            });
    }
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public string GetName() => _user.Name;
    public Guid GetUserId() => _user.UserId;
    
    public long GetTaskId() => _task.Id;
    public string GetTaskTitle() => _task.Title;
    
    public string GetRefreshToken() => _refreshToken.Value;
    
    private async Task StartDatabase(TarefasCrudDbContext dbContext)
    {
        await dbContext.Database.EnsureCreatedAsync();
        
        (var user, _password) = UserBuilder.Build();
        await dbContext.Users.AddAsync(user);
        
        await dbContext.SaveChangesAsync();
        
        _user = await dbContext.Users.FirstAsync(u => u.Email == user.Email);
        
        var task = TaskBuilder.Build(_user);
        _refreshToken = RefreshTokenBuilder.Build(_user);

        await dbContext.Tasks.AddAsync(task);
        await dbContext.RefreshTokens.AddAsync(_refreshToken);
        
        await dbContext.SaveChangesAsync();
        
        _task = await dbContext
            .Tasks
            .AsNoTracking()
            .FirstAsync(t => t.Active && t.UserId == user.Id);
    }
    private async Task InitializeContainer()
    {
        await _dbContainer.StartAsync();
        var masterConnectionString = _dbContainer.GetConnectionString();
        var connectionStringBuilder = new SqlConnectionStringBuilder(masterConnectionString)
        {
            InitialCatalog = "db_tarefascrud"
        };
        _connectionString = connectionStringBuilder.ToString();
    }
}