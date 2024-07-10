using Microsoft.AspNetCore.Mvc;
using Singlearn.Data;
using Microsoft.EntityFrameworkCore;
using Singlearn.Models.Entities;
using System.Linq;
using System.Threading.Tasks;
using Singlearn.Models;
using Singlearn.ViewModels;

namespace Singlearn.Controllers
{
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public StaffController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<IActionResult> Home(string id)
        {
            // Set the staff ID in ViewData for use in the view
            ViewData["StaffId"] = id;

            var subjectsWithClassId = await dbContext.Subjects
                .Join(
                    dbContext.SubjectTeacherClasses,
                    s => s.subject_id,
                    stc => stc.subject_id,
                    (s, stc) => new {
                        Subject = s,
                        SubjectTeacherClass = stc
                    }
                )
                .Where(joined => joined.SubjectTeacherClass.teacher_id == id)
                .Select(joined => new SubjectViewModel
                {
                    subject_id = joined.Subject.subject_id,
                    name = joined.Subject.name,
                    academic_level = joined.Subject.academic_level,
                    image = joined.Subject.image,
                    no_chapters = joined.Subject.no_chapters,
                    year = joined.Subject.year,
                    class_id = joined.SubjectTeacherClass.class_id // Include class_id from SubjectTeacherClass
                })
                .ToListAsync();

            return View(subjectsWithClassId);
        }

        public async Task<IActionResult> GetChapters(int subject_id, string class_id)
        {
            // Set the subject ID and class ID in ViewData for use in the view
            ViewData["SubjectId"] = subject_id;
            ViewData["ClassId"] = class_id;

            var subject_name = await dbContext.Subjects
                .Where(s => s.subject_id.Equals(subject_id))
                .Select(s => s.name)
                .FirstOrDefaultAsync();

            // Pass the data to the view

            var chapters = await dbContext.ChapterNames
                .Where(c => c.subject_id.Equals(subject_id))
                .Select(c => new ChapterViewModel
                {
                    chapter_name_id = c.chapter_name_id,
                    name = c.name,
                    chapter_id = c.chapter_id,
                    subject_id = c.subject_id,
                })
                .ToListAsync();
            ViewData["SubjectName"] = subject_name;

            return View("SubjectMain", chapters);
        }

        public async Task<IActionResult> MaterialsBySubject(int subject_id, int chapter_id, string class_id) 
        {
            var materials = await dbContext.Materials
                .Where(m => m.subject_id == subject_id && m.chapter_id == chapter_id && m.class_id == class_id)
                .ToListAsync();

            return View("ChapterMain", materials);
        }

        public async Task<IActionResult> Announcement()
        {
            var announcements = await dbContext.Announcement.ToListAsync();
            return View(announcements);
        }

        // GET: Create announcement
        public IActionResult CreateAnnouncement()
        {
            return View();
        }

        // POST: Create announcement
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnnouncement(Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(announcement);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Announcement));
            }
            return View(announcement);
        }

        // GET: Edit announcement
        public async Task<IActionResult> EditAnnouncement(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await dbContext.Announcement.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }
            return View(announcement);
        }

        // POST: Edit announcement
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAnnouncement(int id, Announcement announcement)
        {
            if (id != announcement.announcement_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(announcement);
                    await dbContext.SaveChangesAsync();
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
                return RedirectToAction(nameof(Announcement));
            }
            return View(announcement);
        }

        // GET: Delete announcement
        public async Task<IActionResult> DeleteAnnouncement(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await dbContext.Announcement
                .FirstOrDefaultAsync(m => m.announcement_id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // POST: Delete announcement
        [HttpPost, ActionName("DeleteAnnouncement")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var announcement = await dbContext.Announcement.FindAsync(id);
            dbContext.Announcement.Remove(announcement);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Announcement));
        }

        private bool AnnouncementExists(int id)
        {
            return dbContext.Announcement.Any(e => e.announcement_id == id);
        }

        // Other existing actions
        public IActionResult HomeworkHubSubject()
        {
            return View();
        }

        public IActionResult HomeworkHubClass()
        {
            return View();
        }

        public IActionResult HomeworkHubHomework()
        {
            return View();
        }

        public IActionResult HomeworkHubSubmission()
        {
            return View();
        }
    }
}
