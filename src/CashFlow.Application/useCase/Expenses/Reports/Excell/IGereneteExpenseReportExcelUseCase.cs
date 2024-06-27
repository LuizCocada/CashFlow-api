namespace CashFlow.Application.useCase.Expenses.Reports.Excell;


public interface IGereneteExpenseReportExcelUseCase
{
    public Task<byte[]> Execute(DateOnly month);
}