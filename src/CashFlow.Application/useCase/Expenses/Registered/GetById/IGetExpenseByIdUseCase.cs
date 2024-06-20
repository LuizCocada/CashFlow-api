using CashFlow.Communication.Responses;

namespace CashFlow.Application.useCase.Expenses.Registered.GetById;
public interface IGetExpenseByIdUseCase
{
    Task<ResponseExpenseByIdJson> Execute( long id );
}
