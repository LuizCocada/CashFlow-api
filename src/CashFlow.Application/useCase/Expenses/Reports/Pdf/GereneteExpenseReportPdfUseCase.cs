using System.Reflection;
using CashFlow.Application.useCase.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Repositories;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Fonts;
using CashFlow.Domain.Reports;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace CashFlow.Application.useCase.Expenses.Reports.Pdf;

public class GereneteExpenseReportPdfUseCase : IGereneteExpenseReportPdfUseCase
{
    private readonly IExpenseReadOnlyRepository _repository;
    private readonly string _currentSymbol = "€";
    private readonly int HEIGHT_ROW = 25;

    public GereneteExpenseReportPdfUseCase(IExpenseReadOnlyRepository repository)
    {
        _repository = repository;
        GlobalFontSettings.FontResolver = new ExpenseReportFontResolver();
    }
    
    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _repository.GetByDate(month);
        
        if (expenses.Count < 0)
        {
            return [];
        }

        var document = CreateDocument(month);
        var page = CreatePage(document);

        CreateHeaderWithProfilePhotoAndName(page);
        
        var totalExpenses = expenses.Sum(expense => expense.Amount);
        CreateTotalSpentSection(page, month, totalExpenses);

        foreach (var expense in expenses)
        {
            var table = CreateExpenseTable(page);

            var row = table.AddRow();
            row.Height = HEIGHT_ROW;

            AddExpenseTitle(row.Cells[0], expense.Title);
            AddHeaderForAmount(row.Cells[3]);

            row = table.AddRow();
            row.Height = HEIGHT_ROW;
            
            row.Cells[0].AddParagraph(expense.Date.ToString("D"));
            SetStyleBaseForExpensesInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = "20";
            
            row.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseForExpensesInformation(row.Cells[1]);

            row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
            SetStyleBaseForExpensesInformation(row.Cells[2]);

            AddAmountForExpense(row.Cells[3], expense.Amount);

            if (string.IsNullOrWhiteSpace(expense.Description) == false)
            {
                var description = table.AddRow();
                description.Height = HEIGHT_ROW;

                description.Cells[0].AddParagraph(expense.Description);
                description.Cells[0].Format.Font = new Font { Name = FontHelper.WorksansRegular, Size = "10", Color = ColorsHelper.BLACK };
                description.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
                description.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                description.Cells[0].MergeRight = 2; 
                description.Cells[0].Format.LeftIndent = "20";

                row.Cells[3].MergeDown = 1;
            }
            AddWhiteSpace(table);
        }
        
        return RenderDocument(document);
    }

    private void AddExpenseTitle(Cell cell, string expenseTitle)
    {
        cell.AddParagraph(expenseTitle);
        cell.Format.Font = new Font { Name = FontHelper.RalewayBlack, Size = "14", Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2; 
        cell.Format.LeftIndent = "20"; 
    }

    private void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportMessages.AMOUNT);
        cell.Format.Font = new Font { Name = FontHelper.RalewayBlack, Size = "14", Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }
    
    private void CreateHeaderWithProfilePhotoAndName(Section page)
    {
        var table = page.AddTable();
        
        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "Logo", "logo-profile.png");
        
        row.Cells[0].AddImage(pathFile);
        row.Cells[1].AddParagraph("Hey, Luiz Ribeiro");
        row.Cells[1].Format.Font = new Font { Name = FontHelper.RalewayBlack, Size = 16 };
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }

    private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();

        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";
        
        var title = string.Format(ResourceReportMessages.TOTAL_SPENT_IN, month.ToString("Y"));

        paragraph.AddFormattedText(title, new Font{Name = FontHelper.DefaultFont, Size = 15});
        
        paragraph.AddLineBreak();
        
        paragraph.AddFormattedText($"{totalExpenses} {_currentSymbol}", new Font {Name = FontHelper.WorksansBlack, Size = 50});
    }

    private void SetStyleBaseForExpensesInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.WorksansRegular, Size = 12, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddAmountForExpense(Cell cell, decimal expenseAmount)
    {
        cell.AddParagraph($"-{expenseAmount} {_currentSymbol}");
        cell.Format.Font = new Font { Name = FontHelper.WorksansRegular, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }
    
    private Document CreateDocument(DateOnly month)
    {
        var document = new Document
        {
            Info =
            {
                Title = $"{ResourceReportMessages.EXPENSES_FOR} {month.ToString("Y")}",
                Author = "Luiz Ribeiro"
            }
        };

        var styles = document.Styles["Normal"];
        styles!.Font.Name = FontHelper.RalewayRegular;//fonte padrão, caso nao especifique
        
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

    private Table CreateExpenseTable(Section page)
    {
       var table = page.AddTable();

       table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
       table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
       table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
       table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

       return table;
    }
    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document,
        };
        
        renderer.RenderDocument();
        
        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }
}