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
