using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Dtos;
using CommonTestUtilities.Tokens;
using Shouldly;
using TarefasCrud.Domain.Dtos;
using TarefasCrud.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.RoutineTask.GetTasks;

public class GetTasksTest : TarefasCrudClassFixture
{
    private const string METHOD = "/api/tasks";

    private readonly Guid _userId;

    public GetTasksTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userId = factory.GetUserId();
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);

        var filter = new FilterTasksDto
        {
            IsCompleted = false
        };
        var query = FilterTasksDtoBuilder.BuildQuery(filter);
        
        // Act
        var response = await DoGet($"{METHOD}/{query}", token: token);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        
        responseData.RootElement.GetProperty("tasks").GetArrayLength().ShouldBeGreaterThan(0);
    }
}