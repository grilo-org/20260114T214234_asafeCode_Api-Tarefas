using System.Net;
using CommonTestUtilities.Tokens;
using Shouldly;

namespace WebApi.Test.User.Profile;


public class GetUserProfileInvalidTokenTest : TarefasCrudClassFixture
{
    private const string Method = "user";
    public GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory){}

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var response = await DoGet(Method, token: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }  
    [Fact]
    public async Task Error_Token_Empty()
    {
        var response = await DoGet(Method, token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }    
    [Fact]
    public async Task Token_With_User_Not_Found()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
        var response = await DoGet(Method, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}


