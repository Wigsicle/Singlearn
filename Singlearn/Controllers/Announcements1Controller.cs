using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            var staffId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var subjects = _context.SubjectTeacherClasses
                .Where(stc => stc.teacher_id == staffId)
                .Select(stc => stc.Subject)
                .Distinct()
                .ToList();

            ViewData["Subjects"] = new SelectList(subjects, "subject_id", "name");

            var classes = _context.SubjectTeacherClasses
                .Where(stc => stc.teacher_id == staffId)
                .Select(stc => stc.Class)
                .Distinct()
                .ToList();

            ViewData["Classes"] = new SelectList(classes, "class_id", "name");

            // Pass the staff information
            var staffName = User.Identity.Name; // Assuming the user name is the staff name
            ViewData["StaffName"] = staffName;
            ViewData["StaffId"] = staffId;

            return View();
        }

        // POST: Announcements1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("announcement_id,subject_id,staff_id,title,description,image,date,url,category,status")] Announcement announcement, int[] class_id)
        {
            if (ModelState.IsValid)
            {
                // Set the current date
                announcement.date = DateTime.Now;

                // Get the logged-in staff ID
                var staffId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (staffId != null)
                {
                    announcement.staff_id = staffId;

                    // Add the announcement to the context
                    _context.Add(announcement);
                    await _context.SaveChangesAsync();

                    // Save the selected classes
                    foreach (var classId in class_id)
                    {
                        var subjectTeacherClass = new SubjectTeacherClass
                        {
                            subject_id = announcement.subject_id.HasValue ? announcement.subject_id.Value : 0,
                            teacher_id = announcement.staff_id,
                            class_id = classId.ToString() // Assuming class_id is a string
                        };
                        _context.Add(subjectTeacherClass);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            var loggedInStaffId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var subjects = _context.SubjectTeacherClasses
                .Where(stc => stc.teacher_id == loggedInStaffId)
                .Select(stc => stc.Subject)
                .Distinct()
                .ToList();

            ViewData["Subjects"] = new SelectList(subjects, "subject_id", "name", announcement.subject_id);

            var classes = _context.SubjectTeacherClasses
                .Where(stc => stc.teacher_id == loggedInStaffId)
                .Select(stc => stc.Class)
                .Distinct()
                .ToList();

            ViewData["Classes"] = new SelectList(classes, "class_id", "name");

            // Pass the staff information
            var staffName = User.Identity.Name; // Assuming the user name is the staff name
            ViewData["StaffName"] = staffName;
            ViewData["StaffId"] = loggedInStaffId;

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

            var staffId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewData["Subjects"] = new SelectList(
                _context.Subjects
                    .Where(subject => _context.SubjectTeacherClasses
                        .Any(stc => stc.teacher_id == staffId && stc.subject_id == subject.subject_id)),
                "subject_id",
                "name",
                announcement.subject_id
            );

            ViewData["StaffId"] = staffId;
            ViewData["StaffName"] = User.Identity.Name;

            return View(announcement);
        }

        // POST: Announcements1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("announcement_id,subject_id,title,description,image,date,url,category,status")] Announcement announcement, int[] class_id)
        {
            if (id != announcement.announcement_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the announcement
                    announcement.date = DateTime.Now;
                    _context.Update(announcement);
                    await _context.SaveChangesAsync();

                    // Remove existing class associations
                    var existingClasses = _context.SubjectTeacherClasses
                        .Where(stc => stc.teacher_id == announcement.staff_id && stc.subject_id == announcement.subject_id)
                        .ToList();

                    _context.SubjectTeacherClasses.RemoveRange(existingClasses);
                    await _context.SaveChangesAsync();

                    // Add the selected classes
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

            var staffId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewData["Subjects"] = new SelectList(
                _context.Subjects
                    .Where(subject => _context.SubjectTeacherClasses
                        .Any(stc => stc.teacher_id == staffId && stc.subject_id == subject.subject_id)),
                "subject_id",
                "name",
                announcement.subject_id
            );

            ViewData["StaffId"] = staffId;
            ViewData["StaffName"] = User.Identity.Name;

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
