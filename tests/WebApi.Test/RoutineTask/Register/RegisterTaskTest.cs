using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.RoutineTask.Register;

public class RegisterTaskTest : TarefasCrudClassFixture
{
    private const string METHOD = "task";

    private readonly Guid _userId;

    public RegisterTaskTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userId = factory.GetUserId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestTaskJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("title").GetString().ShouldBe(request.Title);
        responseData.RootElement.GetProperty("id").GetInt64().ShouldBeGreaterThan(0);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Title_Empty(string culture)
    {
        var request = RequestTaskJsonBuilder.Build();
        request.Title = string.Empty;

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);

        var response = await DoPost(method: METHOD, request: request, token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TASK_TITLE_EMPTY", new CultureInfo(culture));

        errors.ShouldHaveSingleItem(); errors.ShouldContain(jsonElement => jsonElement.GetString()!.Equals(expectedMessage));
    }
}