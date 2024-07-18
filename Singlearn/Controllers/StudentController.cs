using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Singlearn.Data;
using Singlearn.Models.Entities;
using Singlearn.ViewModels;

namespace Singlearn.Controllers
{
    public class StudentController : Controller
    {

        private readonly ApplicationDbContext dbContext;

        public StudentController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<IActionResult> Home(string id)
        {
            // Set the student ID in ViewData for use in the view
            ViewData["StudentId"] = id;

            // Query the database to get the class ID for the student
            var classId = await dbContext.Students
                .Where(s => s.student_id == id)
                .Select(s => s.class_id)
                .FirstOrDefaultAsync();

            // Pass the retrieved classId to the view
            ViewData["ClassId"] = classId;

            // Query for subject IDs associated with the class ID
            var subject_ids = await dbContext.SubjectTeacherClasses
                .Where(stc => stc.class_id == classId)
                .Select(stc => stc.subject_id)
                .ToListAsync();

            // Query subjects based on the retrieved subject IDs
            var subjects = await dbContext.Subjects
                .Where(s => subject_ids.Contains(s.subject_id))
                .Select(s => new SubjectViewModel
                {
                    subject_id = s.subject_id,
                    name = s.name,
                    academic_level = s.academic_level,
                    image = s.image,
                    no_chapters = s.no_chapters,
                    year = s.year,
                    class_id = classId
                })
                .ToListAsync();

            // Query announcements
            var announcements = await dbContext.Announcements
                .Where(a => a.category == "News" || a.category == "Events")
                .Select(a => new AnnouncementViewModel
                {
                    AnnouncementId = a.announcement_id,
                    Title = a.title,
                    Category = a.category,
                    Status = a.status,
                    Image = a.image,
                    Description = a.description,
                    Date = a.date,
                    SubjectId = a.subject_id,
                    StaffId = a.staff_id,
                    ClassId = a.class_id,
                    Url = a.url,
                })
                .ToListAsync();

            var viewModel = new HomepageViewModel
            {
                Subjects = subjects,
                Announcements = announcements
            };

            return View(viewModel);
        }

        public async Task<IActionResult> SubjectIndex(int subject_id, string class_id)
        {
            ViewData["SubjectId"] = subject_id;
            ViewData["ClassId"] = class_id;

            var subject_name = await dbContext.Subjects
                .Where(s => s.subject_id.Equals(subject_id))
                .Select(s => s.name)
                .FirstOrDefaultAsync();

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



            var announcements = await dbContext.Announcements
                .Where(a => a.subject_id == subject_id && a.class_id.Equals(class_id))
                .ToListAsync();

            var staff_name = await dbContext.Staff
                .Where(s => s.staff_id.Equals(s.staff_id))
                .Select(s => s.name)
                .FirstOrDefaultAsync();

            ViewData["StaffName"] = staff_name;

            ViewData["SubjectName"] = subject_name;

            var stc = await dbContext.SubjectTeacherClasses
                .FirstOrDefaultAsync(stc => stc.subject_id == subject_id && stc.class_id.Equals(class_id));

            var stcTemplate = await dbContext.STCTemplates
                .FirstOrDefaultAsync(st => st.stc_id == stc.stc_id);

            var template = await dbContext.Templates
                .FirstOrDefaultAsync(t => t.template_id == stcTemplate.template_id);

            var viewModel = new SubjectViewModel
            {
                subject_id = subject_id,
                class_id = class_id,
                TemplateViewName = template?.view_name,
                Chapters = chapters,
                Announcements = announcements,
                Materials = new List<Material>()
            };

            return View("SubjectMain", viewModel);
        }

        public async Task<IActionResult> MaterialsBySubject(int subject_id, int chapter_id, string class_id)
        {
            var materials = await dbContext.Materials
                .Where(m => m.subject_id == subject_id && m.chapter_id == chapter_id && m.class_id == class_id)
                .ToListAsync();

            return View("ChapterMain", materials);
        }

        public async Task<IActionResult> GetMaterial(int material_id)
        {
            // Fetch the material by material_id
            var material = await dbContext.Materials
                .FirstOrDefaultAsync(m => m.material_id == material_id);

            if (material == null)
            {
                return NotFound();
            }

            return View("Material", material);
        }

        public IActionResult profile()
        {
            return View();
        }

        public IActionResult lessonmain()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> profile(string id)
        {
            var student = await dbContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View("Profile", student);
        }
    }
}
