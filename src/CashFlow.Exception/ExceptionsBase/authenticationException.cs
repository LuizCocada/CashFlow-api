using System.Net;

namespace CashFlow.Exception.ExceptionsBase;

public class authenticationException : CashFlowException
{
    public authenticationException() : base(ResourceErrorMessages.INVALID_EMAIL_OR_PASSWORD)
    {
    }
    
    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}