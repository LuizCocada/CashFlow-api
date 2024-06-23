using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.Api.Filter;

//precisa ser referenciado no program.cs.
public class ExceptionFilter : IExceptionFilter
{
    public void OnException( ExceptionContext context )
    {
        if( context.Exception is CashFlowException )
        {
            HandleProjectException(context);
        }
        else
        {
            ThrowUnkowError(context);
        }
    }

    private void HandleProjectException( ExceptionContext context )
    {
        var cashflowException = (CashFlowException) context.Exception;
        var errorResponse = new ResponseErrorJson(cashflowException.GetErrors());

        context.HttpContext.Response.StatusCode = cashflowException.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }

    //se as exceções nao for nenhuma de que tenha sido tratada na classe HandleProjectException retorne um error desconhecido;
    private void ThrowUnkowError( ExceptionContext context )
    {
        var errorResponse = new ResponseErrorJson(ResourceErrorMessages.UNKNOW_ERROR);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}
