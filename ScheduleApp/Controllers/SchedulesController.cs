using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScheduleApp.Models;

namespace ScheduleApp.Controllers;

public class TimetableGridEntry
{
    public Schedule Record { get; set; } = null!;
}

public class SchedulesController : Controller
{
    private readonly IsttpContext _context;

    public SchedulesController(IsttpContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "Name");
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FullName");
        return View(new Schedule { RepeatInterval = 7 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EntryId,SubjectId,TeacherId,Link,StartDate,EndDate,TimeStart,TimeFinish,RepeatInterval")] Schedule schedule)
    {
        ModelState.Remove("Subject");
        ModelState.Remove("Teacher");

        if (ModelState.IsValid)
        {
            _context.Add(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "Name", schedule.SubjectId);
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FullName", schedule.TeacherId);
        return View(schedule);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule == null) return NotFound();

        ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "Name", schedule.SubjectId);
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FullName", schedule.TeacherId);
        return View(schedule);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("EntryId,SubjectId,TeacherId,Link,StartDate,EndDate,TimeStart,TimeFinish,RepeatInterval")] Schedule schedule)
    {
        if (id != schedule.EntryId) return NotFound();

        ModelState.Remove("Subject");
        ModelState.Remove("Teacher");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(schedule);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Schedules.Any(e => e.EntryId == schedule.EntryId)) return NotFound();
                else throw;
            }
            return RedirectToAction("Index", "Home");
        }
        ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "Name", schedule.SubjectId);
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FullName", schedule.TeacherId);
        return View(schedule);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule != null)
        {
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index", "Home");
    }
}