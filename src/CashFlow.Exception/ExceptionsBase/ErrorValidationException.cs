namespace CashFlow.Exception.ExceptionsBase;
public class ErrorValidationException : CashFlowException
{
    public List<string> Errors { get; set; } = [];

    public ErrorValidationException( List<string> errorMessages ) : base(string.Empty)
    {
        Errors = errorMessages;
    }
};

