using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.useCase.Login;

public interface ILoginUsecase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestLogin request);
}