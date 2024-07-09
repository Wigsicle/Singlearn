using Microsoft.AspNetCore.Mvc;
using Singlearn.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Singlearn.Models.Entities;
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

        public IActionResult profile()
        {
            return View();
        }

        public IActionResult announcement()
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

            var announcements = await dbContext.Announcements
                .Where(a => a.subject_id == subjectId)
                .ToListAsync();

            var materials = await dbContext.Materials
                .Where(m => m.subject_id == subjectId)
                .ToListAsync();

            var viewModel = new SubjectViewModel
            {
                SubjectTeacherClass = stc,
                TemplateViewName = stcTemplate.Template.view_name,
                Announcements = announcements,
                Materials = materials
            };

            ViewBag.SubjectTeacherClasses = stc;
            return View(viewModel);
        }




    }
}
