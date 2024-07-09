using CashFlow.Domain.Enums;
using CashFlow.Domain.Extensions;
using ClosedXML.Excel;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories;

namespace CashFlow.Application.useCase.Expenses.Reports.Excell;

public class GereneteExpenseReportExcelUseCase : IGereneteExpenseReportExcelUseCase
{
    private readonly IExpenseReadOnlyRepository _repository;
    private readonly string CURRENT_SYMBOL = "€";
    public GereneteExpenseReportExcelUseCase(IExpenseReadOnlyRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _repository.GetByDate(month);
        if (expenses.Count == 0)
        {
            return [];
        }
        
        using var workBook = new XLWorkbook();
         
        workBook.Author="Luiz Ribeiro";
        
        month.ToString("Y");
        
        var worksheet = workBook.Worksheets.Add(month.ToString("Y"));
        
        InsertHeader(worksheet);

        var row = 2;
        foreach (var expense in expenses)
        {
            worksheet.Cell($"A{row}").Value = expense.Title;
            worksheet.Cell($"B{row}").Value = expense.Date;
            worksheet.Cell($"C{row}").Value = expense.PaymentType.PaymentTypeToString();

            worksheet.Cell($"D{row}").Value = expense.Amount;
            worksheet.Cell($"D{row}").Style.NumberFormat.Format = $"-{CURRENT_SYMBOL} #,##0.00";
            
            worksheet.Cell($"E{row}").Value = expense.Description;
            row++;
        }

        worksheet.Columns().AdjustToContents();
        
        var file = new MemoryStream();
        workBook.SaveAs(file);

        return file.ToArray();
    }
    
    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportMessages.TITLE;
        worksheet.Cell("B1").Value = ResourceReportMessages.DATE;
        worksheet.Cell("C1").Value = ResourceReportMessages.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourceReportMessages.AMOUNT;
        worksheet.Cell("E1").Value = ResourceReportMessages.DESCRIPTION;

        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6");

        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }
}