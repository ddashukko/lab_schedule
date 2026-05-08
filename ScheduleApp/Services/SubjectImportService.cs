using ClosedXML.Excel;
using ScheduleApp.Models;
namespace ScheduleApp.Services;

public class SubjectImportService : IImportService<Subject>
{
    private readonly IsttpContext _context;
    public SubjectImportService(IsttpContext context) => _context = context;

    public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (!stream.CanRead) throw new ArgumentException("Stream is not readable");
        using var workBook = new XLWorkbook(stream);
        var worksheet = workBook.Worksheets.FirstOrDefault();
        if (worksheet is null) return;

        foreach (var row in worksheet.RowsUsed().Skip(1))
        {
            await AddSubjectAsync(row, cancellationToken);
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    private Task AddSubjectAsync(IXLRow row, CancellationToken cancellationToken)
    {
        var name = row.Cell(1).GetValue<string>();
        var subject = new Subject
        {
            Name = name,
            UserId = row.Cell(2).GetValue<int>()
        };
        _context.Subjects.Add(subject);
        return Task.CompletedTask;
    }
}