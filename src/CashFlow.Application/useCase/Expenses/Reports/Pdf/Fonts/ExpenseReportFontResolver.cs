using System.Reflection;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Fonts;
using Font = PdfSharp.Charting.Font;

namespace CashFlow.Application.useCase.Expenses.Reports.Pdf.Fonts;

public class ExpenseReportFontResolver : IFontResolver
{
    public byte[]? GetFont(string faceName)
    {
        var stream = ReadFontFile(faceName);

        if (stream is null)
        {
            stream = ReadFontFile(FontHelper.DefaultFont);
        }

        var lenght = (int)stream!.Length;

        var data = new byte[lenght];

        stream.Read(buffer: data, offset: 0, count: lenght);

        return data;
    }
    
    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo(familyName);
    }
    
    private Stream? ReadFontFile(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();//retorna a referencia/dll do projeto Application.
        return assembly.GetManifestResourceStream($"CashFlow.Application.useCase.Expenses.Reports.Pdf.Fonts.{faceName}.ttf");//retorna uma string podendo ser nula caso nao ache o arquivo da fonte
    }                                                    
}
