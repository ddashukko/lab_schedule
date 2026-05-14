using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleApp.Models;
using System.Globalization;

namespace ScheduleApp.Controllers;

public class ChartsController : Controller
{
    private readonly IsttpContext _context;

    public ChartsController(IsttpContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> AssignmentsBySubject()
    {
        var data = await _context.Assignments
            .Include(a => a.Subject)
            .GroupBy(a => a.Subject.Name)
            .Select(g => new {
                subjectName = g.Key ?? "Без предмета",
                count = g.Count()
            })
            .ToListAsync();

        return Json(data);
    }

    [HttpGet]
    public async Task<IActionResult> AssignmentsByMonth()
    {
        var assignments = await _context.Assignments
            .Where(a => a.Deadline.HasValue)
            .ToListAsync();

        var data = assignments
            .GroupBy(a => a.Deadline.Value.Month)
            .Select(g => new {
                monthName = CultureInfo.GetCultureInfo("uk-UA").DateTimeFormat.GetMonthName(g.Key),
                count = g.Count()
            })
            .ToList();

        return Json(data);
    }

    [HttpGet]
    public async Task<IActionResult> AssignmentsByStatus()
    {
        var data = await _context.Assignments
            .GroupBy(a => a.Status)
            .Select(g => new {
                status = g.Key ?? "Active",
                count = g.Count()
            })
            .ToListAsync();

        return Json(data);
    }
}