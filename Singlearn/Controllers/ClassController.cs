using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Singlearn.Data;
using Singlearn.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Singlearn.Controllers
{
    public class ClassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Classes.ToListAsync());
        }


        // GET: Classes/Details/id
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes.FirstOrDefaultAsync(m => m.class_id == id);
            if (classes == null)
            {
                return NotFound();
            }

            return View(classes);
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
            ViewBag.TeacherList = new SelectList(_context.Staff, "staff_id", "name");
            return View();
        }

        // POST: Classes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("class_id,name,teacher_id,academic_level,year")] Class classes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classes);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Class created successfully!";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Error creating material. Please try again.";
            ViewBag.TeacherList = new SelectList(_context.Staff, "staff_id", "name", @classes.teacher_id);
            return View(classes);
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }
            ViewBag.TeacherList = new SelectList(_context.Staff, "staff_id", "name", @class.teacher_id);
            return View(@class);
        }

        // POST: Classes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("class_id,name,teacher_id,academic_level,year")] Class @class)
        {
            if (id != @class.class_id)
            {
                TempData["ErrorMessage"] = "Class not found.";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@class);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Class updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(@class.class_id))
                    {
                        TempData["ErrorMessage"] = "Class not found.";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            TempData["ErrorMessage"] = "Error updating material. Please try again.";
            ViewBag.TeacherList = new SelectList(_context.Staff, "staff_id", "name", @class.teacher_id);
            return View(@class);
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes
                .FirstOrDefaultAsync(m => m.class_id == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class != null)
            {
                _context.Classes.Remove(@class);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Class deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Error deleting class. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClassExists(string id)
        {
            return _context.Classes.Any(e => e.class_id == id);
        }
    }
}