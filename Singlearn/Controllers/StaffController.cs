using Microsoft.AspNetCore.Mvc;
using Singlearn.Data;
using Microsoft.EntityFrameworkCore;
using Singlearn.Models;
using Singlearn.ViewModels;
using Singlearn.Models.Entities;
using Microsoft.AspNetCore.Hosting;

namespace Singlearn.Controllers
{
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment hostingEnvironment;

        public StaffController(ApplicationDbContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            this.dbContext = dbContext;
            this.hostingEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Home(string id)
        {
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
                    class_id = joined.SubjectTeacherClass.class_id
                })
                .ToListAsync();

            return View(subjectsWithClassId);
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

        public IActionResult profile()
        {
            return View();
        }

        public async Task<IActionResult> staff_subject(string id)
        {
            /*if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Staff id is required");
            }*/
            ViewData["StaffId"] = id;
            var subject = await dbContext.Subjects
                .Join(
                dbContext.SubjectTeacherClasses,
                s => s.subject_id,
                stc => stc.subject_id, (s, stc) => new { Subject = s, SubjectTeacherClass = stc })
                .Where(joined => joined.SubjectTeacherClass.teacher_id == id)
                .Select(joined => new SubjectViewModel
                {
                    subject_id = joined.Subject.subject_id,
                    name = joined.Subject.name,
                    academic_level = joined.Subject.academic_level,
                    image = joined.Subject.image,
                    no_chapters = joined.Subject.no_chapters,
                    year = joined.Subject.year
                })
                .ToListAsync();

            return View(subject);
        }

        [HttpGet]
        public async Task<IActionResult> TemplateEditor(string teacher_id)
        {
            try
            {
                var subjects = await dbContext.Subjects
                    .Join(dbContext.SubjectTeacherClasses,
                        subject => subject.subject_id,
                        stc => stc.subject_id,
                        (subject, stc) => new { Subject = subject, stc.teacher_id })
                    .Where(result => result.teacher_id.Equals(teacher_id))
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
                Console.WriteLine($"Error in TemplateEditor: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetClassesForSubject(int subject_id, string teacher_id)
        {
            var classes = await dbContext.SubjectTeacherClasses
                .Where(stc => stc.subject_id == subject_id && stc.teacher_id.Equals(teacher_id))
                .Join(dbContext.Classes,
                      stc => stc.class_id,
                      c => c.class_id,
                      (stc, c) => c)
                .Distinct()
                .ToListAsync();

            return Json(classes);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTemplateSelection(int subject_id, string class_id, int template_id)
        {
            try
            {
                var stc = await dbContext.SubjectTeacherClasses
                    .FirstOrDefaultAsync(stc => stc.subject_id == subject_id && stc.class_id.Equals(class_id));

                if (stc != null)
                {
                    var stcTemplate = await dbContext.STCTemplates
                        .FirstOrDefaultAsync(st => st.stc_id == stc.stc_id);

                    if (stcTemplate != null)
                    {
                        stcTemplate.template_id = template_id;
                        dbContext.STCTemplates.Update(stcTemplate);
                    }
                    else
                    {
                        stcTemplate = new STCTemplate
                        {
                            stc_id = stc.stc_id,
                            template_id = template_id
                        };
                        dbContext.STCTemplates.Add(stcTemplate);
                    }

                    await dbContext.SaveChangesAsync();
                }
                else
                {
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
        public async Task<IActionResult> LoadTemplatePreview(int template_id, int subject_id, string class_id)
        {
            try
            {
                var template = await dbContext.Templates.FirstOrDefaultAsync(t => t.template_id == template_id);
                if (template == null)
                {
                    Console.WriteLine("Template not found.");
                    return Content("Template not found.");
                }
                Console.WriteLine($"Template found: {template.view_name}");

                var stc = await dbContext.SubjectTeacherClasses
                    .FirstOrDefaultAsync(stc => stc.subject_id == subject_id && stc.class_id == class_id);

                var subject_name = await dbContext.Subjects
                .Where(s => s.subject_id.Equals(subject_id))
                .Select(s => s.name)
                .FirstOrDefaultAsync();

                ViewData["SubjectName"] = subject_name;

                var viewModel = new SubjectViewModel
                {
                    subject_id = subject_id,
                    class_id = class_id,
                    name = subject_name,
                    TemplateViewName = template.view_name
                };

                return PartialView($"~/Views/Templates/{template.view_name}.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadTemplatePreview: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
