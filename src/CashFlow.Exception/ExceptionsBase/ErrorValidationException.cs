using System.Net;

namespace CashFlow.Exception.ExceptionsBase;
public class ErrorValidationException: CashFlowException
{
    private readonly List<string> _errors;

    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public ErrorValidationException( List<string> errorMessages ) : base(string.Empty)
    {
        _errors = errorMessages;
    }

    public override List<string> GetErrors()
    {
        return _errors;
    }
};

