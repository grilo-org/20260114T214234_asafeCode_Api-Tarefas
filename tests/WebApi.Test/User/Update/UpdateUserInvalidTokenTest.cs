using System.Net;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;

namespace WebApi.Test.User.Update;

public class UpdateUserInvalidTokenTest : TarefasCrudClassFixture
{
    private const string Method = "user";
    public UpdateUserInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory){}

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var response = await DoPut(Method, request, token: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }  
    [Fact]
    public async Task Error_Token_Empty()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var response = await DoPut(Method, request, token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }    
    [Fact]
    public async Task Token_With_User_Not_Found()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
        var response = await DoPut(Method, request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}