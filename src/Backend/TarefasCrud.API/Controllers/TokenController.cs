using Microsoft.AspNetCore.Mvc;
using TarefasCrud.Application.UseCases.Token.RefreshToken;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Communication.Responses;

namespace TarefasCrud.API.Controllers;

public class TokenController : TarefasCrudControllerBase
{
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken([FromServices] IUseRefreshTokenUseCase useCase,
        [FromBody] RequestNewTokenJson request)
    {
        var response = await useCase.Execute(request);
        return Ok(response);
    }
}