using System.Net.Mime;
using CashFlow.Application.useCase.Expenses.Reports.Excell;
using CashFlow.Application.useCase.Expenses.Reports.Pdf;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    [HttpGet("Excel")] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel
    (
        [FromHeader] DateOnly month,
        [FromServices] IGereneteExpenseReportExcelUseCase useCase 
    )
    {
        byte[] file =  await useCase.Execute(month);
        
        if (file.Length > 0)
        {
            return File(file, MediaTypeNames.Application.Octet, "report.xlsx");
        }

        return NoContent();
    }

    [HttpGet("Pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdf
    (
        [FromQuery] DateOnly month,
        [FromServices] IGereneteExpenseReportPdfUseCase usecase
    )
    {
        byte[] file = await usecase.Execute(month);

        if (file.Length > 0)
        {
            return File(file, MediaTypeNames.Application.Pdf, "report.pdf");
        }

        return NoContent();
    }
}