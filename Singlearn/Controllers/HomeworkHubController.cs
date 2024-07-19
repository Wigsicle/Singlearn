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
            }

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

            if (submission == null)
            {
                return NotFound();
            }

            return View(submission);
        }

        [HttpPost]
        public async Task<IActionResult> staff_marking(FileSubmissionViewModel model)
        {
            var submission = await dbContext.Submissions
                .FirstOrDefaultAsync(s => s.submission_id == model.submission_id);

            if (submission == null)
            {
                return NotFound();
            }

            submission.feedback = model.feedback;
            submission.grade = model.grade;

            if (model.annotatedFilename != null && model.annotatedFilename.Length > 0)
            {
                submission.annotatedFilename = await GetPdfFileBytesAsync(model.annotatedFilename);
            }

            dbContext.Update(submission);
            await dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Submission marked successfully!";
            return RedirectToAction("staff_submission", new { homeworkId = submission.homework_id, classId = submission.class_id });
        }

        public async Task<IActionResult> student_homework(string studentId)
        {
            var student = await dbContext.Students.SingleOrDefaultAsync(s => s.student_id == studentId);
            if (student == null)
            {
                return NotFound();
            }

            var homeworks = await dbContext.Homeworks
                .Join(
                dbContext.SubjectTeacherClasses,
                h=> h.subject_id,
                stc => stc.subject_id,
                (h, stc) => new { h, stc })
                .Where(hstc => hstc.stc.class_id == student.class_id && hstc.stc.teacher_id == hstc.stc.teacher_id)
                .Select(hstc => hstc.h)
                .ToListAsync();

            return View(homeworks);
        }

        [HttpGet]
        public async Task<IActionResult> student_upload(int homeworkId)
        {

            var homework = await dbContext.Homeworks.SingleOrDefaultAsync(h => h.homework_id == homeworkId);
            if (homework == null)
            {
                return NotFound();
            }

            var model = new UploadHomeworkViewModel
            {
                student_id = HttpContext.Session.GetString("student_id"),
                homework_id = homework.homework_id,
                subject_id = homework.subject_id,
                title = homework.title,
                description = homework.description,
               
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> student_upload(UploadHomeworkViewModel model)
        {
            var studentId = HttpContext.Session.GetString("student_id");

            var student = await dbContext.Students.SingleOrDefaultAsync(s => s.student_id.Equals(studentId));

            var submission = new Submission
            {
               
                class_id = student.class_id,
                homework_id = model.homework_id,
                feedback = "",
                grade = "",
                status = "Submitted",
                visibility = true,

            };

            if (model.file != null && model.file.Length > 0)
            {
                submission.originalFilename = await GetPdfFileBytesAsync(model.file);
                submission.annotatedFilename = await GetPdfFileBytesAsync(model.file);

            }

            dbContext.Submissions.Add(submission);
            await dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Homework submitted successfully!";

            return RedirectToAction("student_homework", "HomeworkHub", new { studentId = student.student_id });
        }

        [HttpGet]
        public async Task<IActionResult> student_review(int homeworkId)
        {
            var submission = await dbContext.Submissions
                .FirstOrDefaultAsync(s => s.homework_id == homeworkId);

            if (submission == null)
            {
                return NotFound();
            }

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
    }
}
