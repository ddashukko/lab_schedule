using ScheduleApp.Models;
namespace ScheduleApp.Services;

public class SubjectDataPortServiceFactory : IDataPortServiceFactory<Subject>
{
    private readonly IsttpContext _context;
    public SubjectDataPortServiceFactory(IsttpContext context) => _context = context;

    public IImportService<Subject> GetImportService(string contentType)
    {
        if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || 
            contentType is "application/vnd.ms-excel.sheet.macroEnabled.12" ||
            contentType is "application/octet-stream")
        {
            return new SubjectImportService(_context);
        }
        throw new NotImplementedException($"No import service for {contentType}");
    }

    public IExportService<Subject> GetExportService(string contentType)
    {
        if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            return new SubjectExportService(_context);
        throw new NotImplementedException($"No export service for {contentType}");
    }
}