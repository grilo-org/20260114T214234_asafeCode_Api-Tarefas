using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using TarefasCrud.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update;

public class UpdateUserTest : TarefasCrudClassFixture
{
    private const string Method = "user";
    private readonly Guid _userId;
    public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userId = factory.GetUserId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);

        var response = await DoPut(Method, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Name_Empty(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        var response = await DoPut(Method, request: request, token: token, culture: culture);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var error = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));
        
        error.ShouldSatisfyAllConditions(() =>
        {
            error.ShouldHaveSingleItem();
            error.ShouldContain(jsonElement => jsonElement.GetString()!.Equals(expectedMessage));
        });
    }    
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Email_Empty(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;
        
        var response = await DoPut(Method, request: request, token: token, culture: culture);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var error = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_EMPTY", new CultureInfo(culture));
        
        error.ShouldSatisfyAllConditions(() =>
        {
            error.ShouldHaveSingleItem();
            error.ShouldContain(jsonElement => jsonElement.GetString()!.Equals(expectedMessage));
        });
    }    
}