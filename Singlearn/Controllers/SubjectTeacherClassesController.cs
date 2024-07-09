using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Singlearn.Data;
using Singlearn.Models.Entities;

namespace Singlearn.Controllers
{
    public class SubjectTeacherClassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectTeacherClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubjectTeacherClasses
        public async Task<IActionResult> Index()
        {
            return View(await _context.SubjectTeacherClasses.ToListAsync());
        }

        // GET: SubjectTeacherClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjectTeacherClass = await _context.SubjectTeacherClasses
                .FirstOrDefaultAsync(m => m.stc_id == id);
            if (subjectTeacherClass == null)
            {
                return NotFound();
            }

            return View(subjectTeacherClass);
        }

        // GET: SubjectTeacherClasses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SubjectTeacherClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("stc_id,subject_id,teacher_id,class_id")] SubjectTeacherClass subjectTeacherClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subjectTeacherClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subjectTeacherClass);
        }

        // GET: SubjectTeacherClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjectTeacherClass = await _context.SubjectTeacherClasses.FindAsync(id);
            if (subjectTeacherClass == null)
            {
                return NotFound();
            }
            return View(subjectTeacherClass);
        }

        // POST: SubjectTeacherClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("stc_id,subject_id,teacher_id,class_id")] SubjectTeacherClass subjectTeacherClass)
        {
            if (id != subjectTeacherClass.stc_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subjectTeacherClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectTeacherClassExists(subjectTeacherClass.stc_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subjectTeacherClass);
        }

        // GET: SubjectTeacherClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjectTeacherClass = await _context.SubjectTeacherClasses
                .FirstOrDefaultAsync(m => m.stc_id == id);
            if (subjectTeacherClass == null)
            {
                return NotFound();
            }

            return View(subjectTeacherClass);
        }

        // POST: SubjectTeacherClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subjectTeacherClass = await _context.SubjectTeacherClasses.FindAsync(id);
            if (subjectTeacherClass != null)
            {
                _context.SubjectTeacherClasses.Remove(subjectTeacherClass);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectTeacherClassExists(int id)
        {
            return _context.SubjectTeacherClasses.Any(e => e.stc_id == id);
        }
    }
}
