using ScheduleApp.Models;
namespace ScheduleApp.Services;

public interface IImportService<TEntity> where TEntity : Entity
{
    Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
}