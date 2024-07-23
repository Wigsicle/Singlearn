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

        [HttpGet]
        public async Task<IActionResult> Home()
        {
            try
            {
                var studentId = HttpContext.Session.GetString("student_id");
                var classId = HttpContext.Session.GetString("class_id");
                if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(classId))
                {
                    return RedirectToAction("Login", "Auth"); // Redirect to login if student_id or class_id is not found
                }

                // Query for subject IDs associated with the class ID
                var subject_ids = await dbContext.SubjectTeacherClasses
                    .Where(stc => stc.class_id == classId)
                    .Select(stc => stc.subject_id)
                    .ToListAsync();

                var className = await dbContext.Classes
                    .Where(c => c.class_id == classId)
                    .Select(c => c.name)
                    .FirstOrDefaultAsync();

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
                        class_id = classId,
                        class_name = className
                    })
                    .ToListAsync();

                // Query announcements
                var announcements = await dbContext.Announcements
                    .Where(a => a.category == "News" || a.category == "Events")
                    .Join(dbContext.Staff,
                          a => a.staff_id,
                          s => s.staff_id,
                          (a, s) => new AnnouncementViewModel
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
                              StaffName = s.name, // Include staff name here
                              ClassId = a.class_id,
                              Url = a.url,
                          })
                    .OrderByDescending(a => a.Date) // Assuming you want the latest announcements, order by date in descending order
                    .Take(5) // Limit the result to 5
                    .ToListAsync();


                var viewModel = new HomepageViewModel
                {
                    Subjects = subjects,
                    Announcements = announcements
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Student Home: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SubjectIndex(int subject_id)
        {
            try
            {
                var studentId = HttpContext.Session.GetString("student_id");
                var classId = HttpContext.Session.GetString("class_id");
                if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(classId))
                {
                    return RedirectToAction("Login", "Auth"); // Redirect to login if student_id or class_id is not found
                }

                ViewData["SubjectId"] = subject_id;
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
                    .Where(a => a.subject_id == subject_id && a.class_id.Equals(classId))
                    .ToListAsync();

                var staffId = await dbContext.SubjectTeacherClasses
                .Where(stc => stc.subject_id == subject_id && stc.class_id == classId)
                .Select(stc => stc.teacher_id)
                .FirstOrDefaultAsync();

                var staff_name = await dbContext.Staff
                    .Where(s => s.staff_id == staffId)
                    .Select(s => s.name)
                    .FirstOrDefaultAsync();


                ViewData["StaffName"] = staff_name;

                ViewData["SubjectName"] = subject_name;

                var stc = await dbContext.SubjectTeacherClasses
                    .FirstOrDefaultAsync(stc => stc.subject_id == subject_id && stc.class_id.Equals(classId));

                var stcTemplate = await dbContext.STCTemplates
                    .FirstOrDefaultAsync(st => st.stc_id == stc.stc_id);

                var template = await dbContext.Templates
                    .FirstOrDefaultAsync(t => t.template_id == stcTemplate.template_id);

                var viewModel = new SubjectViewModel
                {
                    subject_id = subject_id,
                    class_id = classId,
                    TemplateViewName = template?.view_name,
                    Chapters = chapters,
                    Announcements = announcements,
                    Materials = new List<Material>()
                };

                return View("SubjectMain", viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Student SubjectIndex: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> MaterialsBySubject(int subject_id, int chapter_id)
        {
            try
            {
                var studentId = HttpContext.Session.GetString("student_id");
                var classId = HttpContext.Session.GetString("class_id");
                if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(classId))
                {
                    return RedirectToAction("Login", "Auth"); // Redirect to login if student_id or class_id is not found
                }

                var materials = await dbContext.Materials
                    .Where(m => m.subject_id == subject_id && m.chapter_id == chapter_id && m.class_id == classId)
                    .ToListAsync();

                return View("ChapterMain", materials);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Student MaterialsBySubject: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMaterial(int material_id)
        {
            try
            {
                var studentId = HttpContext.Session.GetString("student_id");
                var classId = HttpContext.Session.GetString("class_id");
                if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(classId))
                {
                    return RedirectToAction("Login", "Auth"); // Redirect to login if student_id or class_id is not found
                }

                // Fetch the material by material_id
                var material = await dbContext.Materials
                    .FirstOrDefaultAsync(m => m.material_id == material_id);

                if (material == null)
                {
                    return NotFound();
                }

                return View("Material", material);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Student GetMaterial: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
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
