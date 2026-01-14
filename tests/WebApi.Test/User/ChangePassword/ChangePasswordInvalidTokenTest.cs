using System.Net;
using CommonTestUtilities.Tokens;
using Shouldly;
using TarefasCrud.Communication.Requests;

namespace WebApi.Test.User.ChangePassword;

public class ChangePasswordInvalidTokenTest : TarefasCrudClassFixture
{
    private const string Method = "user";
    public ChangePasswordInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory){}

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = new RequestChangePasswordJson();
        var response = await DoPut(Method, request: request ,token: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }  
    
    [Fact]
    public async Task Error_Token_Empty()
    {
        var request = new RequestChangePasswordJson();
        var response = await DoPut(method: Method, request: request ,token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }    
    
    [Fact]
    public async Task Token_With_User_Not_Found()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
        var request = new RequestChangePasswordJson();
        var response = await DoPut(method: Method, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}