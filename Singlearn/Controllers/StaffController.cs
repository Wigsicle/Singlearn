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
        public async Task<IActionResult> TemplateEditor(int teacherId)
        {
            try
            {


                ViewBag.Subjects = await dbContext.SubjectTeacherClasses
                    .Include(stc => stc.Subject)
                    .Where(stc => stc.teacher_id.Equals(teacherId))
                    .Select(stc => stc.Subject)
                    .Distinct()
                    .ToListAsync();

                ViewBag.Classes = await dbContext.SubjectTeacherClasses
                    .Include(stc => stc.Class)
                    .Where(stc => stc.teacher_id.Equals(teacherId))
                    .Select(stc => stc.Class)
                    .Distinct()
                    .ToListAsync();

                ViewBag.Templates = await dbContext.Templates.ToListAsync();

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
        public IActionResult LoadTemplatePreview(int templateId)
        {
            var template = dbContext.Templates.FirstOrDefault(t => t.template_id == templateId);
            if (template == null)
            {
                return Content("Template not found.");
            }
            return PartialView($"Templates/{template.view_name}");
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
