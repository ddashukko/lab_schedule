using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScheduleApp.Models;

namespace ScheduleApp.Controllers;

public class AssignmentsController : Controller
{
    private readonly IsttpContext _context;

    public AssignmentsController(IsttpContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var assignments = _context.Assignments.Include(a => a.Subject);
        return View(await assignments.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var assignment = await _context.Assignments
            .Include(a => a.Subject)
            .FirstOrDefaultAsync(m => m.TaskId == id);

        if (assignment == null) return NotFound();

        return View(assignment);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TaskId,SubjectId,Description,Deadline")] Assignment assignment)
    {
        if (ModelState.IsValid)
        {
            _context.Add(assignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "Name", assignment.SubjectId);
        return View(assignment);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var assignment = await _context.Assignments.FindAsync(id);
        if (assignment == null) return NotFound();

        ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "Name", assignment.SubjectId);
        return View(assignment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("TaskId,SubjectId,Description,Deadline")] Assignment assignment)
    {
        if (id != assignment.TaskId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(assignment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(assignment.TaskId)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["SubjectId"] = new SelectList(_context.Subjects, "SubjectId", "Name", assignment.SubjectId);
        return View(assignment);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var assignment = await _context.Assignments
            .Include(a => a.Subject)
            .FirstOrDefaultAsync(m => m.TaskId == id);

        if (assignment == null) return NotFound();

        return View(assignment);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var assignment = await _context.Assignments.FindAsync(id);
        if (assignment != null)
        {
            _context.Assignments.Remove(assignment);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AssignmentExists(int id)
    {
        return _context.Assignments.Any(e => e.TaskId == id);
    }
}