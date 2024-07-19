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
using Singlearn.ViewModels;

namespace Singlearn.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Announcements
        public async Task<IActionResult> Index()
        {
            // Retrieve the staff ID from session
            var staffId = HttpContext.Session.GetString("staff_id");

            if (string.IsNullOrEmpty(staffId))
            {
                return Unauthorized(); // Handle this case as needed
            }

            // Filter the announcements based on the staff ID
            var announcements = await _context.Announcements
                .Where(a => a.staff_id == staffId)
                .Select(a => new AnnouncementViewModel
                {
                    AnnouncementId = a.announcement_id,
                    SubjectId = a.subject_id,
                    StaffId = a.staff_id,
                    ClassId = a.class_id,
                    Title = a.title,
                    Description = a.description,
                    Image = a.image,
                    Date = a.date,
                    Url = a.url,
                    Category = a.category,
                    Status = a.status
                })
                .ToListAsync();

            ViewData["StaffId"] = staffId;
            return View(announcements);
        }

        // GET: Announcements/Details/
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

        // GET: Announcements/Create
        [HttpGet]
        [Route("Announcements/Create")]
        public IActionResult Create()
        {
            var staffId = HttpContext.Session.GetString("staff_id");
            if (string.IsNullOrEmpty(staffId))
            {
                return Unauthorized(); // Handle unauthorized access as needed
            }

            var announcement = new Announcement
            {
                staff_id = staffId,
                date = DateTime.Now
            };

            // Fetch subjects associated with the staff
            var subjects = _context.SubjectTeacherClasses
                .Where(stc => stc.teacher_id == staffId)
                .Select(stc => new { stc.subject_id, SubjectName = _context.Subjects.First(s => s.subject_id == stc.subject_id).name })
                .Distinct()
                .ToList();

            ViewBag.Subjects = new SelectList(subjects, "subject_id", "SubjectName");

            return View(announcement);
        }



        // POST: Announcements/Create
        [HttpPost]
        [Route("Announcements/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("subject_id, class_id, title, description, image, url, category, status, staff_id")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
/*                var staffId = HttpContext.Session.GetString("staff_id");
                announcement.staff_id = staffId;*/
                announcement.date = DateTime.Now;

                _context.Announcements.Add(announcement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(announcement);
        }


        // GET: Announcements/Edit/
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

            var staffId = HttpContext.Session.GetString("staff_id");
            if (string.IsNullOrEmpty(staffId))
            {
                return Unauthorized(); // Handle unauthorized access as needed
            }

            // Fetch subjects associated with the staff
            var subjects = _context.SubjectTeacherClasses
                .Where(stc => stc.teacher_id == staffId)
                .Select(stc => new { stc.subject_id, SubjectName = _context.Subjects.First(s => s.subject_id == stc.subject_id).name })
                .Distinct()
                .ToList();

            ViewBag.Subjects = new SelectList(subjects, "subject_id", "SubjectName", announcement.subject_id);

            // Fetch classes associated with the subject
            var classes = _context.SubjectTeacherClasses
                .Where(stc => stc.teacher_id == staffId && stc.subject_id == announcement.subject_id)
                .Select(stc => new { stc.class_id, ClassName = _context.Classes.First(c => c.class_id == stc.class_id).name })
                .ToList();

            ViewBag.Classes = new MultiSelectList(classes, "class_id", "ClassName", announcement.class_id);
            return View(announcement);
        }

        // POST: Announcements/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("announcement_id,title,description,image,url,category,status,subject_id,class_id,staff_id,date")] Announcement announcement)
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

            var staffId = HttpContext.Session.GetString("staff_id");
            if (string.IsNullOrEmpty(staffId))
            {
                return Unauthorized(); // Handle unauthorized access as needed
            }

            // Fetch subjects associated with the staff
            var subjects = _context.SubjectTeacherClasses
                .Where(stc => stc.teacher_id == staffId)
                .Select(stc => new { stc.subject_id, SubjectName = _context.Subjects.First(s => s.subject_id == stc.subject_id).name })
                .Distinct()
                .ToList();

            ViewBag.Subjects = new SelectList(subjects, "subject_id", "SubjectName", announcement.subject_id);

            // Fetch classes associated with the subject
            var classes = _context.SubjectTeacherClasses
                .Where(stc => stc.teacher_id == staffId && stc.subject_id == announcement.subject_id)
                .Select(stc => new { stc.class_id, ClassName = _context.Classes.First(c => c.class_id == stc.class_id).name })
                .ToList();

            ViewBag.Classes = new MultiSelectList(classes, "class_id", "ClassName", announcement.class_id);
            return View(announcement);
        }

        // GET: Announcements/Delete/
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


        // POST: Announcements/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement != null)
            {
                _context.Announcements.Remove(announcement);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool AnnouncementExists(int id)
        {
            return _context.Announcements.Any(e => e.announcement_id == id);
        }

        [HttpGet]
        public JsonResult GetSubjectsByStaff(string staff_id)
        {
            var subjects = _context.SubjectTeacherClasses
                .Where(stc => stc.teacher_id == staff_id)
                .Select(stc => new { value = stc.subject_id, text = _context.Subjects.First(s => s.subject_id == stc.subject_id).name })
                .Distinct()
                .ToList();

            return Json(subjects);
        }

        [HttpGet]
        public JsonResult GetClassesByStaff(string staff_id)
        {
            var classes = _context.SubjectTeacherClasses
                .Where(stc => stc.teacher_id == staff_id)
                .Select(stc => new { value = stc.class_id, text = _context.Classes.First(c => c.class_id == stc.class_id).name })
                .Distinct()
                .ToList();

            return Json(classes);
        }
    }
}
