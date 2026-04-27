using Microsoft.AspNetCore.Mvc;
using ScheduleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ScheduleApp.Controllers;

public class TeachersController : Controller
{
    private readonly IsttpContext _context;

    public TeachersController(IsttpContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Teachers.ToListAsync());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TeacherId,FullName,Link")] Teacher teacher)
    {
        if (ModelState.IsValid)
        {
            var existingTeacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.FullName == teacher.FullName);

            if (existingTeacher == null)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
            }
            
            return Content("<script>window.close();</script>", "text/html");
        }
        return View(teacher);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return NotFound();
        return View(teacher);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("TeacherId,FullName,Link")] Teacher teacher)
    {
        if (id != teacher.TeacherId) return NotFound();
        if (ModelState.IsValid)
        {
            _context.Update(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(teacher);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var teacher = await _context.Teachers.FirstOrDefaultAsync(m => m.TeacherId == id);
        if (teacher == null) return NotFound();
        return View(teacher);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher != null)
        {
            var relatedSchedules = _context.Schedules.Where(s => s.TeacherId == id);
            _context.Schedules.RemoveRange(relatedSchedules);
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}