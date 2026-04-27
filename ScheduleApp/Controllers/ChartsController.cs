using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleApp.Models;

namespace ScheduleApp.Controllers;

public class ChartsController : Controller
{
    private readonly IsttpContext _context;

    public ChartsController(IsttpContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<JsonResult> AssignmentsBySubject()
    {
        // Спочатку витягуємо дані
        var assignments = await _context.Assignments
            .Include(a => a.Subject)
            .ToListAsync();

        // Потім групуємо (щоб PostgreSQL не видавав помилок)
        var data = assignments
            .GroupBy(a => a.Subject?.Name ?? "Без предмета")
            .Select(g => new
            {
                subjectName = g.Key,
                count = g.Count()
            })
            .ToList();

        return Json(data);
    }

    [HttpGet]
    public async Task<JsonResult> AssignmentsByMonth()
    {
        // Витягуємо лише ті, де є дедлайн
        var assignments = await _context.Assignments
            .Where(a => a.Deadline.HasValue)
            .ToListAsync();

        var ukCulture = new System.Globalization.CultureInfo("uk-UA");

        var data = assignments
            .GroupBy(a => a.Deadline!.Value.ToString("MMMM", ukCulture))
            .Select(g => new
            {
                monthName = g.Key,
                count = g.Count(),
                monthOrder = g.First().Deadline!.Value.Month
            })
            .OrderBy(r => r.monthOrder)
            .ToList();

        return Json(data);
    }
}