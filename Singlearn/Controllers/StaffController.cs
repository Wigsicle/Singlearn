using Microsoft.AspNetCore.Mvc;
using SinglearnWeb.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Singlearn.Models.Entities;

namespace SinglearnWeb.Controllers
{
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public StaffController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public IActionResult home()
        {
            return View();
        }

        public IActionResult profile()
        {
            return View();
        }

        public IActionResult homeworkhub_subject()
        {
            return View();
        }

        public IActionResult homeworkhub_class()
        {
            return View();
        }

        public IActionResult homeworkhub_homework()
        {
            return View();
        }

        public IActionResult homeworkhub_submission()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TemplateEditor(string teacherId)
        {
            try
            {
                var subjects = await dbContext.Subjects
                .Join(dbContext.SubjectTeacherClasses,
                subject => subject.subject_id,
                stc => stc.subject_id,
                (subject, stc) => new { Subject = subject, stc.teacher_id })
                .Where(result => result.teacher_id.Equals(teacherId))
                .Select(result => result.Subject)
                .Distinct()
                .ToListAsync();

                ViewBag.Subjects = subjects;

                var classes = await dbContext.SubjectTeacherClasses
                .Include(stc => stc.Class)
                .Where(stc => stc.teacher_id.Equals(teacherId))
                .Select(stc => stc.Class)
                .Distinct()
                .ToListAsync();

                ViewBag.Classes = classes;

                var templates = await dbContext.Templates.ToListAsync();
                ViewBag.Templates = templates;

                Console.WriteLine($"Subjects found: {subjects.Count}");
                foreach (var subject in subjects)
                {
                    Console.WriteLine($"Subject: {subject}");
                }

                return View();
            }
            catch (Exception ex)
            {
                // Log the exception and show a meaningful error message
                Console.WriteLine($"Error in TemplateEditor: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> SaveTemplateSelection(int subjectId, int classId, int templateId)
        {
            var stc = await dbContext.SubjectTeacherClasses
                .FirstOrDefaultAsync(stc => stc.subject_id == subjectId && stc.class_id == classId);

            if (stc != null)
            {
                var stcTemplate = await dbContext.STCTemplates
                    .FirstOrDefaultAsync(st => st.stc_id == stc.stc_id);

                if (stcTemplate != null)
                {
                    stcTemplate.template_id = templateId;
                }
                else
                {
                    stcTemplate = new STCTemplate
                    {
                        stc_id = stc.stc_id,
                        template_id = templateId
                    };
                    dbContext.STCTemplates.Add(stcTemplate);
                }

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("TemplateEditor", new { teacherId = stc.teacher_id });
        }

        [HttpGet]
        public async Task<IActionResult> LoadTemplatePreview(int templateId, int subjectId, int classId)
        {
            try
            {
                var template = await dbContext.Templates.FirstOrDefaultAsync(t => t.template_id == templateId);
                if (template == null)
                {
                    Console.WriteLine("Template not found.");
                    return Content("Template not found.");
                }
                Console.WriteLine($"Template found: {template.view_name}");

                // Fetch SubjectTeacherClass data
                var stc = await dbContext.SubjectTeacherClasses
                    .Include(stc => stc.Subject)
                    .Include(stc => stc.Class)
                    .FirstOrDefaultAsync(stc => stc.subject_id == subjectId && stc.class_id == classId);

                if (stc == null)
                {
                    return Content("Subject or Class not found.");
                }

                return PartialView($"~/Views/Templates/{template.view_name}.cshtml", stc);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadTemplatePreview: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SubjectPage(int subjectId, int classId)
        {
            var stc = await dbContext.SubjectTeacherClasses
                .Include(stc => stc.Subject)
                .Include(stc => stc.Class)
                .Include(stc => stc.Staff)
                .FirstOrDefaultAsync(stc => stc.subject_id == subjectId && stc.class_id == classId);

            if (stc == null)
            {
                return NotFound("Subject or Class not found.");
            }

            var stcTemplate = await dbContext.STCTemplates
                .Include(st => st.Template)
                .FirstOrDefaultAsync(st => st.stc_id == stc.stc_id);

            if (stcTemplate == null)
            {
                return NotFound("Template not found.");
            }

            ViewBag.SubjectTeacherClasses = stc;
            return View((object)stcTemplate.Template.view_name);
        }

    }
}
