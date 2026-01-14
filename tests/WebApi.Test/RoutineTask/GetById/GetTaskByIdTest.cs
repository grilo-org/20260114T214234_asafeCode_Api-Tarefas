using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using TarefasCrud.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.RoutineTask.GetById;

public class GetTaskByIdTest : TarefasCrudClassFixture
{
    private const string METHOD = "task";

    private readonly Guid _userId;
    private readonly long _taskId;
    private readonly string _taskTitle;

    public GetTaskByIdTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userId = factory.GetUserId();
        _taskId = factory.GetTaskId();
        _taskTitle = factory.GetTaskTitle();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
        
        var response = await DoGet($"{METHOD}/{_taskId}", token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var id = responseData.RootElement.GetProperty("id").GetInt64();
        var title = responseData.RootElement.GetProperty("title").GetString();
        
        id.ShouldBeGreaterThan(0); id.ShouldBe(_taskId);
        title.ShouldNotBeNullOrWhiteSpace(); title.ShouldBe(_taskTitle);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Task_Not_Found(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);

        var response = await DoGet(method: $"{METHOD}/{1000}", token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TASK_NOT_FOUND", new CultureInfo(culture));

        errors.ShouldHaveSingleItem(); errors.ShouldContain(jsonElement => jsonElement.GetString()!.Equals(expectedMessage));
    }
}