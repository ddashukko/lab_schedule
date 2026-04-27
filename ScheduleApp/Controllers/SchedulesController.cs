using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScheduleApp.Models;
using System.Globalization;

namespace ScheduleApp.Controllers;

public class TimetableGridEntry
{
    public Schedule Record { get; set; } = null!;
    public string SubjectColorAccent { get; set; } = string.Empty;
    public string SubjectColorBackground { get; set; } = string.Empty;
}

public class SchedulesController : Controller
{
    private readonly IsttpContext _context;

    public SchedulesController(IsttpContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var allSchedules = await _context.Schedules
            .Include(s => s.Subject)
            .Include(s => s.Teacher)
            .Include(s => s.Timetable)
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

            if (!grid.ContainsKey(timeKey))
            {
                grid[timeKey] = new Dictionary<DayOfWeek, List<TimetableGridEntry>>();
            }
            if (!grid[timeKey].ContainsKey(dayKey))
            {
                grid[timeKey][dayKey] = new List<TimetableGridEntry>();
            }

            var colors = GenerateStableSubjectColors(schedule.SubjectId);

            grid[timeKey][dayKey].Add(new TimetableGridEntry
            {
                Record = schedule,
                SubjectColorAccent = colors.Accent,
                SubjectColorBackground = colors.Background
            });
        }

        var sortedTimeSlots = uniqueTimeSlots.OrderBy(s => s).ToList();

        ViewBag.GridData = grid;
        ViewBag.SortedTimeSlots = sortedTimeSlots;

        return View(allSchedules);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "Name");
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FullName");
        ViewData["TimetableId"] = new SelectList(_context.Timetables, "TimetableId", "Name");
        
        var model = new Schedule { RepeatInterval = 7 };
       return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EntryId,TimetableId,SubjectId,TeacherId,StartDate,EndDate,TimeStart,TimeFinish,RepeatInterval")] Schedule schedule)
    {
        if (ModelState.IsValid)
        {
            _context.Add(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "Name", schedule.SubjectId);
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FullName", schedule.TeacherId);
        ViewData["TimetableId"] = new SelectList(_context.Timetables, "TimetableId", "Name", schedule.TimetableId);
        return View(schedule);
    }

    private static (string Accent, string Background) GenerateStableSubjectColors(int? subjectId)
    {
        var colorPairs = new List<(string, string)>
        {
            ("#723793", "#dfc9ff"),
            ("#26958d", "#b9f5f0"),
            ("#db5f00", "#ffd4a3"),
            ("#0060db", "#a3d1ff"),
            ("#228B22", "#98FB98")
        };

        int index = Math.Abs((subjectId ?? 0) % colorPairs.Count);
        return colorPairs[index];
    }
}