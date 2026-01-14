using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using TarefasCrud.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.RoutineTask.UpdateTask;

public class UpdateTaskTest : TarefasCrudClassFixture
{
    private const string METHOD = "task";

    private readonly Guid _userId;
    private readonly long _taskId;

    public UpdateTaskTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userId = factory.GetUserId();
        _taskId = factory.GetTaskId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestTaskJsonBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
        
        var response = await DoPut(method: $"{METHOD}/{_taskId}", request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        
        response = await DoGet($"{METHOD}/{_taskId}", token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var id = responseData.RootElement.GetProperty("id").GetInt64();
        var title = responseData.RootElement.GetProperty("title").GetString();
        
        id.ShouldBeGreaterThan(0); id.ShouldBe(_taskId);
        title.ShouldNotBeNullOrWhiteSpace(); title.ShouldBe(request.Title);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Title_Empty(string culture)
    {
        var request = RequestTaskJsonBuilder.Build();
        request.Title = string.Empty;

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);

        var response = await DoPut(method: $"{METHOD}/{_taskId}", request: request, token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TASK_TITLE_EMPTY", new CultureInfo(culture));

        errors.ShouldHaveSingleItem(); errors.ShouldContain(jsonElement => jsonElement.GetString()!.Equals(expectedMessage));
    }
}