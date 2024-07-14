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
    public class Announcements1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public Announcements1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Announcements1
        public async Task<IActionResult> Index()
        {
            return View(await _context.Announcements.ToListAsync());
        }

        // GET: Announcements1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements
                .FirstOrDefaultAsync(m => m.announcement_id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // GET: Announcements1/Create
        public IActionResult Create()
        {
            ViewData["Subjects"] = new SelectList(_context.Subjects, "subject_id", "name");
            ViewData["Staffs"] = new SelectList(_context.Staff, "staff_id", "name");
            return View();
        }

        // POST: Announcements1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("announcement_id,subject_id,staff_id,title,description,image,date,url,category,status")] Announcement announcement, int[] class_id)
        {
            if (ModelState.IsValid)
            {
                // Add the announcement to the context
                _context.Add(announcement);
                await _context.SaveChangesAsync();

                // Save the selected classes
                foreach (var classId in class_id)
                {
                    var subjectTeacherClass = new SubjectTeacherClass
                    {
                        subject_id = announcement.subject_id.Value,
                        teacher_id = announcement.staff_id,
                        class_id = classId.ToString() // Assuming class_id is a string
                    };
                    _context.Add(subjectTeacherClass);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Subjects"] = new SelectList(_context.Subjects, "subject_id", "name", announcement.subject_id);
            ViewData["Staffs"] = new SelectList(_context.Staff, "staff_id", "name", announcement.staff_id);
            return View(announcement);
        }

        // GET: Announcements1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            ViewData["Subjects"] = new SelectList(_context.Subjects, "subject_id", "name", announcement.subject_id);
            ViewData["Staffs"] = new SelectList(_context.Staff, "staff_id", "name", announcement.staff_id);

            return View(announcement);
        }

        // POST: Announcements1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("announcement_id,subject_id,staff_id,class_id,title,description,image,date,url,category,status")] Announcement announcement)
        {
            if (id != announcement.announcement_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(announcement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementExists(announcement.announcement_id))
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
            return View(announcement);
        }

        // GET: Announcements1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements
                .FirstOrDefaultAsync(m => m.announcement_id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // POST: Announcements1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement != null)
            {
                _context.Announcements.Remove(announcement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnouncementExists(int id)
        {
            return _context.Announcements.Any(e => e.announcement_id == id);
        }

        [HttpGet]
        public JsonResult GetClassesByStaff(string staff_id)
        {
            var classes = _context.SubjectTeacherClasses
                .Where(stc => stc.teacher_id == staff_id)
                .Select(stc => new { value = stc.class_id, text = _context.Classes.First(c => c.class_id == stc.class_id).name })
                .ToList();

            return Json(classes);
        }
    }
}
