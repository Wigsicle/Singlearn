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

                var templates = await dbContext.Templates.ToListAsync();
                ViewBag.Templates = templates;

                return View();
            }
            catch (Exception ex)
            {
                // Log the exception and show a meaningful error message
                Console.WriteLine($"Error in TemplateEditor: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetClassesForSubject(int subjectId, string teacherId)
        {
            var classes = await dbContext.SubjectTeacherClasses
                .Include(stc => stc.Class)
                .Where(stc => stc.subject_id == subjectId && stc.teacher_id.Equals(teacherId))
                .Select(stc => stc.Class)
                .Distinct()
                .ToListAsync();

            return Json(classes);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTemplateSelection(int subjectId, string classId, int templateId)
        {
            try
            {
                var stc = await dbContext.SubjectTeacherClasses
                    .FirstOrDefaultAsync(stc => stc.subject_id == subjectId && stc.class_id.Equals(classId));

                if (stc != null)
                {
                    var stcTemplate = await dbContext.STCTemplates
                        .FirstOrDefaultAsync(st => st.stc_id == stc.stc_id);

                    if (stcTemplate != null)
                    {
                        stcTemplate.template_id = templateId;
                        dbContext.STCTemplates.Update(stcTemplate);
                        Console.WriteLine("Template updated successfully.");
                    }
                    else
                    {
                        stcTemplate = new STCTemplate
                        {
                            stc_id = stc.stc_id,
                            template_id = templateId
                        };
                        dbContext.STCTemplates.Add(stcTemplate);
                        Console.WriteLine("Template added successfully.");
                    }

                    await dbContext.SaveChangesAsync();
                    Console.WriteLine("Changes saved to the database.");
                }
                else
                {
                    Console.WriteLine("SubjectTeacherClass not found.");
                    return Json(new { success = false, message = "SubjectTeacherClass not found." });
                }

                return Json(new { success = true, message = "Template saved successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SaveTemplateSelection: {ex.Message}");
                return Json(new { success = false, message = $"Internal server error: {ex.Message}" });
            }
        }




        [HttpGet]
        public async Task<IActionResult> LoadTemplatePreview(int templateId, int subjectId, string classId)
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
                    .FirstOrDefaultAsync(stc => stc.subject_id == subjectId && stc.class_id.Equals(classId));

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
        public async Task<IActionResult> SubjectPage(int subjectId, string classId)
        {
            try
            {
                var stc = await dbContext.SubjectTeacherClasses
                    .Include(stc => stc.Subject)
                    .Include(stc => stc.Class)
                    .Include(stc => stc.Staff)
                    .FirstOrDefaultAsync(stc => stc.subject_id == subjectId && stc.class_id.Equals(classId));

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SubjectPage: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
