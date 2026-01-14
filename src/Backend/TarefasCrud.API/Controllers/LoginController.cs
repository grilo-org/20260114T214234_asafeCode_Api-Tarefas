using Microsoft.AspNetCore.Mvc;
using TarefasCrud.Application.UseCases.Login;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Communication.Responses;

namespace TarefasCrud.API.Controllers;

public class LoginController : TarefasCrudControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] IDoLoginUseCase useCase, 
        [FromBody] RequestLoginJson request)
    {
        var result = await useCase.Execute(request);
        return Ok(result);
    }
}