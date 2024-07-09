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
