using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using ScheduleApp.Models;
namespace ScheduleApp.Services;

public class SubjectExportService : IExportService<Subject>
{
    private readonly IsttpContext _context;
    private static readonly IReadOnlyList<string> HeaderNames = new[] { "Назва", "ID Користувача" };

    public SubjectExportService(IsttpContext context) => _context = context;

    public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (!stream.CanWrite) throw new ArgumentException("Stream is not writable");
        var subjects = await _context.Subjects.ToListAsync(cancellationToken);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Subjects");

        for (int i = 0; i < HeaderNames.Count; i++)
            worksheet.Cell(1, i + 1).Value = HeaderNames[i];
        worksheet.Row(1).Style.Font.Bold = true;

        int rowIndex = 2;
        foreach (var s in subjects)
        {
            worksheet.Cell(rowIndex, 1).Value = s.Name;
            worksheet.Cell(rowIndex, 2).Value = s.UserId;
            rowIndex++;
        }
        workbook.SaveAs(stream);
    }
}