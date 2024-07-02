namespace CashFlow.Application.useCase.Expenses.Reports.Pdf;

public interface IGereneteExpenseReportPdfUseCase
{
    public Task<byte[]> Execute(DateOnly month);
}