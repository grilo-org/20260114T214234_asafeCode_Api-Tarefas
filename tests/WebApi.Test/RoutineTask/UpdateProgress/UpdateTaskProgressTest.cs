using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using TarefasCrud.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.RoutineTask.UpdateProgress;

public class UpdateTaskProgressTest : TarefasCrudClassFixture
{
     private const string METHOD = "task";

    private readonly Guid _userId;
    private readonly long _taskId;

    public UpdateTaskProgressTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userId = factory.GetUserId();
        _taskId = factory.GetTaskId();
    }
    
    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
        
        var response = await DoPut(method: $"{METHOD}/{_taskId}/progress", token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);        
        
        response = await DoPut(method: $"{METHOD}/{_taskId}/progress/decrement", token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        
        response = await DoGet($"{METHOD}/{_taskId}", token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var id = responseData.RootElement.GetProperty("id").GetInt64();
        var progress = responseData.RootElement.GetProperty("progress").GetInt32();
        
        id.ShouldBeGreaterThan(0); id.ShouldBe(_taskId);
        progress.ShouldBe(0);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Decrement_Initial_Progress(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);

        var  response = await DoPut(method: $"{METHOD}/{_taskId}/progress/decrement", token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NOT_DECREMENT_INITIAL_PROGRESS_TASK", new CultureInfo(culture));

        errors.ShouldHaveSingleItem(); errors.ShouldContain(jsonElement => jsonElement.GetString()!.Equals(expectedMessage));
    }
}