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
        //ex está pegando a exception(errors) da classe ErrorValidationException;
        //verifica se o tipo de erro é de validaçao
        if( context.Exception is ErrorValidationException exceptionValidation )
        {
            var errorResponse = new ResponseErrorJson(exceptionValidation.Errors);

            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Result = new BadRequestObjectResult(errorResponse);
        }
        else if(context.Exception is NotFoundException exceptionNotFound)
        {
            var errorResponse = new ResponseErrorJson(exceptionNotFound.Message);

            context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Result = new NotFoundObjectResult(errorResponse);
        }
        else //se entrou aqui é por que a exceção é do tipo CashFlowException. Ela é executada se nao achar nenhum bloco IF passando a classe de ERROR;
        {
            var errorResponse = new ResponseErrorJson(context.Exception.Message);

            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Result = new BadRequestObjectResult(errorResponse);
        }
    }

    //se as exceções nao for nenhuma de que tenha sido tratada na classe HandleProjectException retorne um error desconhecido;
    private void ThrowUnkowError( ExceptionContext context )
    {
        var errorResponse = new ResponseErrorJson(ResourceErrorMessages.UNKNOW_ERROR);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}
