using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Shouldly;
using TarefasCrud.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;

public class RegisterUserTest : TarefasCrudClassFixture
{
    private const string Method = "user";
    
    public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory) {}

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var response = await DoPost(method: Method, request: request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        
        //responseBody com o conteúdo da response e ler com streamAsync 
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        
        //responseBody em um jsonDocument, parse async:
        var responseData = await JsonDocument.ParseAsync(responseBody);
        
        //Json sempre vem em camelCase
        var name = responseData.RootElement.GetProperty("name").GetString();
        var tokens = responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString();
        
        name.ShouldNotBeNullOrWhiteSpace(); name.ShouldBe(request.Name);
        tokens.ShouldNotBeNullOrEmpty();
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Name_Empty(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var response = await DoPost(method: Method, request: request, culture: culture);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var error = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));
        
        error.ShouldHaveSingleItem(); error.ShouldContain(jsonElement => jsonElement.GetString()!.Equals(expectedMessage));
    }

}