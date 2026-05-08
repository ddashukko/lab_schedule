using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleApp.Models;

namespace ScheduleApp.Controllers;

public class HomeController : Controller
{
    private readonly IsttpContext _context;

    public HomeController(IsttpContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var allSchedules = await _context.Schedules
            .Include(s => s.Subject)
            .Include(s => s.Teacher)
            .ToListAsync();

        var grid = new Dictionary<string, Dictionary<DayOfWeek, List<TimetableGridEntry>>>();
        var uniqueTimeSlots = new HashSet<string>();

        foreach (var schedule in allSchedules)
        {
            if (schedule.Subject == null) continue;

            string startTime = schedule.TimeStart?.ToString("HH:mm") ?? "00:00";
            string finishTime = schedule.TimeFinish?.ToString("HH:mm") ?? "00:00";
            string timeKey = $"{startTime}-{finishTime}";
            uniqueTimeSlots.Add(timeKey);

            DayOfWeek dayKey = schedule.StartDate?.DayOfWeek ?? DayOfWeek.Monday;

            if (!grid.ContainsKey(timeKey)) grid[timeKey] = new Dictionary<DayOfWeek, List<TimetableGridEntry>>();
            if (!grid[timeKey].ContainsKey(dayKey)) grid[timeKey][dayKey] = new List<TimetableGridEntry>();

            grid[timeKey][dayKey].Add(new TimetableGridEntry
            {
                Record = schedule
            });
        }

        ViewBag.GridData = grid;
        ViewBag.SortedTimeSlots = uniqueTimeSlots.OrderBy(s => s).ToList();

        return View();
    }

    public IActionResult Statistics() => View();
}