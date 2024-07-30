using CashFlow.Application.useCase.Expenses.Register;
using CashFlow.Application.useCase.Expenses.Registered.DeleteById;
using CashFlow.Application.useCase.Expenses.Registered.GetAll;
using CashFlow.Application.useCase.Expenses.Registered.GetById;
using CashFlow.Application.useCase.Expenses.Registered.Update;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]//precisa de um token valido passado em program.cs
public class ExpensesController: ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredExpenseJson) , StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson) , StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterExpenseUseCase useCase ,
        [FromBody] RequestExpenseJson request )
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty , response);
    }


    [HttpGet]
    [ProducesResponseType(typeof(ResponseExpensesJson) , StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson) , StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> GetAllExpenses( [FromServices] IGetAllExpenseUseCase useCase )
    {
        var response = await useCase.Execute();

        if( response.Expenses.Count != 0 )
            return Ok(response);

        return NoContent();

    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseExpenseByIdJson) , StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson) , StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetExpenseById( 
        [FromRoute] long id ,
        [FromServices] IGetExpenseByIdUseCase useCase )

    {
        var response = await useCase.Execute(id);

        if( response is not null )
        { return Ok(response); }

        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson) , StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteExpenseById(
        [FromRoute] long id ,
        [FromServices] IDeleteExpenseByIdUseCase useCase )

    {

        await useCase.Execute(id);

        return NoContent();
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson) , StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson) , StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateExpenseById(
        [FromServices] IUpdateExpenseUseCase useCase,
        [FromRoute] long id,
        [FromBody] RequestExpenseJson request )
    {
        await useCase.Execute(id, request);

        return NoContent();
    }
}
