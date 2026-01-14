using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;
using Shouldly;

namespace WebApi.Test.User.Profile;

public class GetUserProfileTest : TarefasCrudClassFixture
{
    private const string Method = "user";
    private readonly Guid _userId;
    private readonly string _email;
    private readonly string _name;

    public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userId = factory.GetUserId();
        _email = factory.GetEmail();
        _name = factory.GetName();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userId);
        
        var response = await DoGet(Method, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var name = responseData.RootElement.GetProperty("name").GetString();
        var email = responseData.RootElement.GetProperty("email").GetString();
        
        email.ShouldNotBeNullOrWhiteSpace(); email.ShouldBe(_email);
        name.ShouldNotBeNullOrWhiteSpace(); name.ShouldBe(_name);
    }
}