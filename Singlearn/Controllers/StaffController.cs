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

            // Fetch chapters for the given subject
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

            // Fetch the template associated with the subject and class
            var stc = await dbContext.SubjectTeacherClasses
                .Include(stc => stc.Subject)
                .Include(stc => stc.Class)
                .FirstOrDefaultAsync(stc => stc.subject_id == subject_id && stc.class_id == class_id);

            var stcTemplate = await dbContext.STCTemplates
                .Include(st => st.Template)
                .FirstOrDefaultAsync(st => st.stc_id == stc.stc_id);

            var viewModel = new SubjectViewModel
            {
                SubjectTeacherClass = stc,
                TemplateViewName = stcTemplate?.Template?.view_name,
                Chapters = chapters,
                Announcements = new List<Announcement>(), // Add Announcements if needed
                Materials = new List<Material>() // Add Materials if needed
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


        [HttpPost]
        /*public async Task<IActionResult> homeworkhub_homework([Bind("homework_id,subject_id,title,desciption,startdate,enddate,attachment")]Homework homework)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(homework);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Home", "Staff");
            }
            return View(homework);
        }*/

        [HttpGet]
        public async Task<IActionResult> homeworkhub_homework(int subjectId)
        {
            var homeworks = await dbContext.Homeworks
                .Where(h => h.subject_id == subjectId)
                .Select(h => new HomeworkViewModel
                {
                    homework_id = h.homework_id,
                    subject_id = h.subject_id,
                    title = h.title,
                    description = h.description,
                    startdate = h.startdate,
                    enddate = h.enddate,
                    //attachment = h.attachment
                })
                .ToListAsync();

            ViewData["SubjectId"] = subjectId;

            return View(homeworks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHomework(HomeworkViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFilename = null;
                if (model.attachment != null)
                {
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "attachments");
                    uniqueFilename = Guid.NewGuid().ToString() + "_" + model.attachment.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFilename);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.attachment.CopyToAsync(fileStream);
                    }
                }

                var homework = new Homework
                {
                    subject_id = model.subject_id,
                    title = model.title,
                    description = model.description,
                    startdate = model.startdate,
                    enddate = model.enddate,
                    attachment = uniqueFilename,
                };

                await dbContext.Homeworks.AddAsync(homework);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("homeworkhub_homework", "Staff", new { subjectId = model.subject_id });
            }
            return View(model);
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



        public IActionResult homeworkhub_submission()
        {
            return View();
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
                .Include(stc => stc.Class)
                .Where(stc => stc.subject_id == subject_id && stc.teacher_id.Equals(teacher_id))
                .Select(stc => stc.Class)
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
                    .Include(stc => stc.Subject)
                    .Include(stc => stc.Class)
                    .FirstOrDefaultAsync(stc => stc.subject_id == subject_id && stc.class_id.Equals(class_id));

                if (stc == null)
                {
                    return Content("Subject or Class not found.");
                }

                var announcements = await dbContext.Announcements
                    .Where(a => a.subject_id == subject_id)
                    .ToListAsync();

                var chapters = await dbContext.ChapterNames
                    .Where(c => c.subject_id == subject_id)
                    .ToListAsync();

                var viewModel = new SubjectViewModel
                {
                    SubjectTeacherClass = stc,
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