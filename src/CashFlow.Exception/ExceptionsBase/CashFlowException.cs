namespace CashFlow.Exception.ExceptionsBase;

public abstract class CashFlowException : SystemException
{
    public CashFlowException( string? message ) : base(message) { }
}

