using Microsoft.AspNetCore.Mvc;
using Singlearn.Data;
using Microsoft.EntityFrameworkCore;
using Singlearn.Models;
using Singlearn.ViewModels;
using Singlearn.Models.Entities;

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

    }
}
