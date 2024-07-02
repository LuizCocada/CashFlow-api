using CashFlow.Application.useCase.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Repositories;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Fonts;
using PdfSharp.Snippets.Font;
using CashFlow.Domain.Reports;

namespace CashFlow.Application.useCase.Expenses.Reports.Pdf;

public class GereneteExpenseReportPdfUseCase : IGereneteExpenseReportPdfUseCase
{
    private readonly string CURRENT_SYMBOL = "€";
    private readonly IExpenseReadOnlyRepository _repository;

    public GereneteExpenseReportPdfUseCase(IExpenseReadOnlyRepository repository)
    {
        _repository = repository;
        GlobalFontSettings.FontResolver = new ExoticFontsFontResolver();
    }
    
    public async Task<byte[]> Execute(DateOnly month)
    {
        var expense = await _repository.GetByDate(month);
        
        if (expense.Count < 0)
        {
            return [];
        }

        var document = CreateDocument(month);
        var page = CreatePage(document);
        
        
        return [];
    }
    
    private Document CreateDocument(DateOnly month)
    {
        var document = new Document();
        document.Info.Title = $"{ResourcePaymentTypeMessages.EXPENSES_FOR} {month.ToString("Y")}";
        document.Info.Author = "Luiz Ribeiro";

        var styles = document.Styles["Normal"];
        styles!.Font.Name = FontHelper.RALEWAY_REGULAR;//fonte padrão, caso nao especifique
        
        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();//adiciona uma pagina no documento
        section.PageSetup = document.DefaultPageSetup.Clone();//clone das configuracoes default parametro documento.
        
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }
}