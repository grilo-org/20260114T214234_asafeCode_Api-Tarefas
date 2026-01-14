using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.ChangePassword;

public class ChangePasswordTest : TarefasCrudClassFixture
{
    private const string Method = "user/change-password";
    private readonly string _email;
    private readonly string _password;
    private readonly Guid _userId;

    public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _email = factory.GetEmail();
        _password = factory.GetPassword();
        _userId = factory.GetUserId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = _password;

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);

        var response = await DoPut(method: Method, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginJson()
        {
            Email = _email,
            Password = _password
        };
        
        var loginResponseUnauth = await DoPost(method:"login", loginRequest);
        loginResponseUnauth.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        
        loginRequest.Password = request.NewPassword;
        
        var loginResponseOk = await DoPost(method:"login", loginRequest);
        loginResponseOk.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_NewPassword_Empty(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
        
        var request = new RequestChangePasswordJson
        {
            Password = _password,
            NewPassword = string.Empty
        };
        
        var response = await DoPut(method: Method, request: request, token: token, culture: culture);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var error = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("PASSWORD_EMPTY", new CultureInfo(culture));
        
        error.ShouldHaveSingleItem(); error.ShouldContain(jsonElement => jsonElement.GetString()!.Equals(expectedMessage));
    }
}