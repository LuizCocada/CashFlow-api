using ClosedXML.Excel;
using CashFlow.Domain.Reports;

namespace CashFlow.Application.useCase.Expenses.Reports.Excell;

public class GereneteExpenseReportExcelUseCase : IGereneteExpenseReportExcelUseCase
{
    public async Task<byte[]> Execute(DateOnly month)
    {
        var workBook = new XLWorkbook();
         
        workBook.Author="Luiz Ribeiro";
        
        month.ToString("Y");
        
        var worksheet = workBook.Worksheets.Add(month.ToString("Y"));
        
        InsertHeader(worksheet);

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