using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleApp.Models;

namespace ScheduleApp.Controllers;

public class SubjectsController : Controller
{
    private readonly IsttpContext _context;

    public SubjectsController(IsttpContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Subjects.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var subject = await _context.Subjects
            .FirstOrDefaultAsync(m => m.SubjectId == id);

        if (subject == null) return NotFound();

        return View(subject);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("SubjectId,Name,ColorCode,UserId")] Subject subject)
    {
        if (ModelState.IsValid)
        {
            var existingSubject = await _context.Subjects
                .FirstOrDefaultAsync(s => s.Name == subject.Name);

            if (existingSubject == null)
            {
                _context.Add(subject);
                await _context.SaveChangesAsync();
            }
            return Content("<script>window.close();</script>", "text/html");
        }
        return View(subject);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null) return NotFound();
        
        return View(subject);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("SubjectId,Name,ColorCode,UserId")] Subject subject)
    {
        if (id != subject.SubjectId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(subject);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(subject.SubjectId)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(subject);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var subject = await _context.Subjects
            .FirstOrDefaultAsync(m => m.SubjectId == id);
            
        if (subject == null) return NotFound();

        return View(subject);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject != null)
        {
            _context.Subjects.Remove(subject);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SubjectExists(int id)
    {
        return _context.Subjects.Any(e => e.SubjectId == id);
    }
}