﻿using CashFlow.Application.useCase.Expenses.Register;
using CashFlow.Application.useCase.Expenses.Registered.GetAll;
using CashFlow.Application.useCase.Expenses.Registered.GetById;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterExpenseJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson) , StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> Register( [FromServices] IRegisterExpenseUseCase useCase, [FromBody] RequestRegisterExpenseJson request )
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty , response);
    }


    [HttpGet]
    [ProducesResponseType(typeof(ResponseExpensesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson) , StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> GetAllExpenses( [FromServices] IGetAllExpenseUseCase useCase)
    {
        var response = await useCase.Execute();

        if( response.Expenses.Count != 0 )
            return Ok(response);

        return NoContent();

    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseExpenseByIdJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson) , StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetExpenseById( [FromRoute] long id, [FromServices] IGetExpenseByIdUseCase useCase)
    {
        var response = await useCase.Execute(id);

        if(response is not null)
        { return Ok(response); }

        return NoContent();
    }
}
