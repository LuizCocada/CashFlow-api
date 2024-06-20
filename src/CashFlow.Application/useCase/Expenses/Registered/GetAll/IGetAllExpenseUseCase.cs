using CashFlow.Communication.Responses;

namespace CashFlow.Application.useCase.Expenses.Registered.GetAll;
public interface IGetAllExpenseUseCase
{
    Task<ResponseExpensesJson> Execute();
}
