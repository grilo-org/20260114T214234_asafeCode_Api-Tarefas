using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Shouldly;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest :  TarefasCrudClassFixture
{
    private const string Method = "login";
    private readonly string _email;
    private readonly string _name;
    private readonly string _password;

    public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _email = factory.GetEmail();
        _name = factory.GetName();
        _password = factory.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson()
        {
            Email = _email,
            Password = _password,
        };

        var response = await DoPost(method: Method, request: request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var name = responseData.RootElement.GetProperty("name").GetString();
        var tokens = responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString();

        name.ShouldSatisfyAllConditions(() =>
        {
            name.ShouldNotBeNullOrWhiteSpace();
            name.ShouldBe(_name);
            tokens.ShouldNotBeNullOrEmpty();
        });
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_User(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();
        
        var response = await DoPost(method: Method, request: request, culture: culture);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var error = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));
        
        error.ShouldSatisfyAllConditions(() =>
        {
            error.ShouldHaveSingleItem();
            error.ShouldContain(jsonElement => jsonElement.GetString()!.Equals(expectedMessage));
        });
        
    }

}
