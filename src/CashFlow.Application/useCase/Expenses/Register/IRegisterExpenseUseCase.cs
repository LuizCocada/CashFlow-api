using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;

namespace CashFlow.Application.useCase.Expenses.Register;
public interface IRegisterExpenseUseCase
{
     Task<ResponseRegisterExpenseJson> Execute( RequestRegisterExpenseJson request );
}
