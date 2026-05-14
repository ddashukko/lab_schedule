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
        DateTime today = DateTime.Today;
        int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        DateTime startOfWeek = today.AddDays(-1 * diff).Date;

        var weekDates = new Dictionary<DayOfWeek, DateOnly>();
        for (int i = 0; i < 6; i++)
        {
            DateTime dt = startOfWeek.AddDays(i);
            weekDates[dt.DayOfWeek] = DateOnly.FromDateTime(dt);
        }

        var allSchedules = await _context.Schedules
            .Include(s => s.Subject)
            .Include(s => s.Teacher)
            .ToListAsync();

        var grid = new Dictionary<string, Dictionary<DayOfWeek, List<TimetableGridEntry>>>();
        var uniqueTimeSlots = new HashSet<string>();

        foreach (var schedule in allSchedules)
        {
            if (schedule.Subject == null || schedule.StartDate == null) continue;

            string startTime = schedule.TimeStart?.ToString("HH:mm") ?? "00:00";
            string finishTime = schedule.TimeFinish?.ToString("HH:mm") ?? "00:00";
            string timeKey = $"{startTime}-{finishTime}";

            foreach (var dayKvp in weekDates)
            {
                DayOfWeek dayOfWeek = dayKvp.Key;
                DateOnly currentDate = dayKvp.Value;

                if (currentDate >= schedule.StartDate.Value && 
                    (schedule.EndDate == null || currentDate <= schedule.EndDate.Value))
                {
                    int daysPassed = currentDate.DayNumber - schedule.StartDate.Value.DayNumber;
                    bool shouldAdd = false;

                    if ((schedule.RepeatInterval == null || schedule.RepeatInterval == 0) && daysPassed == 0)
                    {
                        shouldAdd = true;
                    }
                    else if (schedule.RepeatInterval > 0 && daysPassed % schedule.RepeatInterval == 0)
                    {
                        shouldAdd = true;
                    }

                    if (shouldAdd)
                    {
                        uniqueTimeSlots.Add(timeKey);
                        if (!grid.ContainsKey(timeKey)) grid[timeKey] = new Dictionary<DayOfWeek, List<TimetableGridEntry>>();
                        if (!grid[timeKey].ContainsKey(dayOfWeek)) grid[timeKey][dayOfWeek] = new List<TimetableGridEntry>();

                        grid[timeKey][dayOfWeek].Add(new TimetableGridEntry
                        {
                            Record = schedule
                        });
                    }
                }
            }
        }

        ViewBag.GridData = grid;
        ViewBag.SortedTimeSlots = uniqueTimeSlots.OrderBy(s => s).ToList();
        ViewBag.WeekDates = weekDates;

        return View();
    }

    public IActionResult Statistics() => View();
}