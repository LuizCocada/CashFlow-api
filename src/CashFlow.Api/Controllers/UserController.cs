using CashFlow.Application.useCase.Users.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser(
        [FromServices] IRegisterUserUseCase userUseCase,
        [FromBody] RequestRegisterUserJson request
    )
    {
        var response = await userUseCase.Execute(request);

        return Created(string.Empty, response);
    }
}