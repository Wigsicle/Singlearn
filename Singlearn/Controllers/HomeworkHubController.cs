using Microsoft.AspNetCore.Mvc;
using Singlearn.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Singlearn.Models.Entities;
using Singlearn.ViewModels;
using Microsoft.IdentityModel.Tokens;
using Singlearn.Models;
using System.Text;

namespace SinglearnWeb.Controllers
{
    public class HomeworkHubController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment hostingEnvironment;

        public HomeworkHubController(ApplicationDbContext dbContext, IWebHostEnvironment hostingEnvironment)
        {
            this.dbContext = dbContext;
            this.hostingEnvironment = hostingEnvironment;
        }


        /*public IActionResult staff_subject()
        {
            return View();
        }*/

        public async Task<IActionResult> staff_subject(string teacherId)
        {
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
                .Where(joined => joined.SubjectTeacherClass.teacher_id == teacherId)
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

        public async Task<IActionResult> staff_homework(int subjectId)
        {
            var homework = await dbContext.Homeworks.Where(h => h.subject_id == subjectId).ToListAsync();
            ViewData["SubjectId"] = subjectId;

            return View(homework);
        }

        [HttpGet]
        public IActionResult staff_homework_create(int subjectId)
        {
            var model = new HomeworkViewModel 
            { 
                subject_id = subjectId
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> staff_homework_create(HomeworkViewModel model)
        {
            if (ModelState.IsValid)
            {
                var homework = new Homework
                {
                    subject_id = model.subject_id,
                    title = model.title,
                    description = model.description,
                    startdate = model.startdate,
                    enddate = model.enddate,
                };

                if (model.attachment != null && model.attachment.Length > 0)
                {
                    homework.attachment = await GetPdfFileBytesAsync(model.attachment);
                }

                dbContext.Add(homework);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("staff_homework", "HomeworkHub", new { subjectId = model.subject_id });
                /*using (var memoryStream = new MemoryStream())
                {
                    await model.attachment.CopyToAsync(memoryStream);
                    var homework = new Homework
                    {
                        subject_id = model.subject_id,
                        title = model.title,
                        description = model.description,
                        startdate = model.startdate,
                        enddate = model.enddate,
                        attachment = model.attachment != null ? await GetPdfFileBytesAsync(model.attachment) : null
                    };

                    dbContext.Add(homework);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction("staff_homework", "HomeworkHub", new { subjectId = model.subject_id });
                }*/
            }

            // If we reach here, something went wrong, so return to the view with the model
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> staff_homework_edit(int homeworkid)
        {
            var homework = await dbContext.Homeworks.FindAsync(homeworkid);
            var viewModel = new HomeworkViewModel
            {
                homework_id = homework.homework_id,
                subject_id = homework.subject_id,
                title = homework.title,
                description = homework.description,
                startdate = homework.startdate,
                enddate = homework.enddate
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> staff_homework_edit(int homeworkid, HomeworkViewModel homework)
        {
            try
            {
                var existingHomeworks = await dbContext.Homeworks.FindAsync(homework.homework_id);
                existingHomeworks.subject_id = homework.subject_id;
                existingHomeworks.title = homework.title;
                existingHomeworks.description = homework.description;
                existingHomeworks.startdate = homework.startdate;
                existingHomeworks.enddate = homework.enddate;

                // Update PDF file if provided
                if (homework.attachment != null && homework.attachment.Length > 0)
                {
                    existingHomeworks.attachment = await GetPdfFileBytesAsync(homework.attachment);
                }

                dbContext.Update(existingHomeworks);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("staff_homework", "HomeworkHub", new { subjectId = existingHomeworks.subject_id });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomeworkExists(homeworkid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> staff_homework_delete(int homeworkid)
        {

            var homework = await dbContext.Homeworks.FirstOrDefaultAsync(h => h.homework_id == homeworkid);

            dbContext.Homeworks.Remove(homework);
            await dbContext.SaveChangesAsync();
            TempData["SuccessMessage"] = "Homework deleted successfully";
            return RedirectToAction("staff_homework", new { subjectId = homework.subject_id });
        }

        [HttpGet]
        public async Task<IActionResult> staff_class(int homeworkId)
        {
            var model = new HomeworkViewModel
            {
                homework_id = homeworkId
            };

            var homeworkSubmissions = await dbContext.Submissions
                .Where(s => s.homework_id == homeworkId)
                .Join(
                    dbContext.Classes,
                    s => s.class_id,
                    c => c.class_id,
                    (s, c) => new {
                        Submission = s,
                        Class = c
                    }
                )
                .GroupBy(joined => joined.Class.class_id)
                .Select(joined => new ClassesViewModel
                {
                    name = joined.First().Class.name,
                    class_id = joined.First().Class.class_id,   
                    
                })
                .ToListAsync();

            ViewData["HomeworkId"] = homeworkId;

            return View(homeworkSubmissions);
        }

        [HttpGet]
        public async Task<IActionResult> staff_submission(int homeworkId, string classId)
        {


            /*var model = new ClassesViewModel
            {
                class_id = classId
            };*/

            var submissions = await dbContext.Submissions
                .Where(s => s.class_id == classId && s.homework_id == homeworkId) // Filter by class_id
                .Join(
                    dbContext.Students,
                    s => s.class_id,
                    stu => stu.class_id,
                    (s, stu) => new SubmissionViewModel
                    {
                        submission_id = s.submission_id,
                        class_id = s.class_id,
                        homework_id = s.homework_id,
                        originalFilename = Encoding.UTF8.GetString(s.originalFilename), 
                        status = s.status,
                        visibility = s.visibility,
                        student_name = stu.name

                    })
                    .ToListAsync();




            return View(submissions);
        }

        [HttpGet]
        public async Task<IActionResult> staff_marking(int submissionId)
        {
            var submission = await dbContext.Submissions
                .FirstOrDefaultAsync(s => s.submission_id == submissionId);

            return View(submission);
        }

        private bool HomeworkExists(int id)
        {
            return dbContext.Homeworks.Any(e => e.homework_id == id);
        }
        private async Task<byte[]> GetPdfFileBytesAsync(IFormFile pdfFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await pdfFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /*public async Task<IActionResult> ViewAttachment(int submissionId)
        {
            var submission = await dbContext.Submissions
                .Include(s => s.homework_id)
                .FirstOrDefaultAsync(s => s.submission_id == submissionId);

            if (submission == null || submission.originalFilename == null)
            {
                return NotFound();
            }

            return File(submission.originalFilename, "application/pdf");
        }*/

    }
}
