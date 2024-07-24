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
                // Retrieve the student ID and class ID from the session
                var studentId = HttpContext.Session.GetString("student_id");
                var classId = HttpContext.Session.GetString("class_id");
                // Redirect to login if student_id or class_id is not found
                if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(classId))
                {
                    return RedirectToAction("Login", "Auth"); // Redirect to login if student_id or class_id is not found
                }

                // Set the subject ID in the ViewData
                ViewData["SubjectId"] = subject_id;

                // Fetch the subject name based on the subject ID
                var subject_name = await dbContext.Subjects
                    .Where(s => s.subject_id.Equals(subject_id))
                    .Select(s => s.name)
                    .FirstOrDefaultAsync();

                // Fetch the chapters associated with the subject ID
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

                // Fetch the announcements associated with the subject ID and class ID
                var announcements = await dbContext.Announcements
                    .Where(a => a.subject_id == subject_id && a.class_id.Equals(classId))
                    .ToListAsync();

                // Fetch the staff ID associated with the subject ID and class ID
                var staffId = await dbContext.SubjectTeacherClasses
                .Where(stc => stc.subject_id == subject_id && stc.class_id == classId)
                .Select(stc => stc.teacher_id)
                .FirstOrDefaultAsync();

                // Fetch the staff name based on the staff ID
                var staff_name = await dbContext.Staff
                    .Where(s => s.staff_id == staffId)
                    .Select(s => s.name)
                    .FirstOrDefaultAsync();

                // Set the staff name and subject name in the ViewData
                ViewData["StaffName"] = staff_name;
                ViewData["SubjectName"] = subject_name;

                // Fetch the SubjectTeacherClass entity based on subject ID and class ID
                var stc = await dbContext.SubjectTeacherClasses
                    .FirstOrDefaultAsync(stc => stc.subject_id == subject_id && stc.class_id.Equals(classId));

                // Fetch the STCTemplate entity based on stc_id
                var stcTemplate = await dbContext.STCTemplates
                    .FirstOrDefaultAsync(st => st.stc_id == stc.stc_id);

                // Fetch the template based on template_id from the STCTemplate entity
                var template = await dbContext.Templates
                    .FirstOrDefaultAsync(t => t.template_id == stcTemplate.template_id);

                // Create a new SubjectViewModel with the fetched data
                var viewModel = new SubjectViewModel
                {
                    subject_id = subject_id,
                    class_id = classId,
                    TemplateViewName = template?.view_name,
                    Chapters = chapters,
                    Announcements = announcements,
                    Materials = new List<Material>()
                };

                // Return the "SubjectMain" view with the view model
                return View("SubjectMain", viewModel);
            }
            catch (Exception ex)
            {
                // Log the error and return a 500 internal server error status code
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

        [HttpGet]
        public async Task<IActionResult> profile()
        {
            var studentId = HttpContext.Session.GetString("student_id");
            if (string.IsNullOrEmpty(studentId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var student = await dbContext.Students
                .Join(dbContext.Classes,
                      s => s.class_id,
                      c => c.class_id,
                      (s, c) => new StudentProfileViewModel
                      {
                          student_id = s.student_id,
                          name = s.name,
                          contact_no = s.contact_no,
                          class_id = c.class_id
                      })
                .FirstOrDefaultAsync(sp => sp.student_id == studentId);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        public IActionResult lessonmain()
        {
            return View();
        }

    }
}
