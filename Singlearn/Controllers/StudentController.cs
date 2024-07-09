using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Singlearn.Data;
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
                {  // Adjust to use your actual ViewModel or entity
                    subject_id = s.subject_id,
                    name = s.name,
                    academic_level = s.academic_level,
                    image = s.image,
                    no_chapters = s.no_chapters,
                    year = s.year,
                    class_id = classId  // Include the class ID here
                })
                .ToListAsync();

            return View(subjects);
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
