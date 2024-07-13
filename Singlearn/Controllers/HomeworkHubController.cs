using Microsoft.AspNetCore.Mvc;
using Singlearn.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Singlearn.Models.Entities;
using Singlearn.ViewModels;

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

        [HttpGet]
        public async Task<IActionResult> Index(int subjectId, int? homeworkId = null)
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

            /*HomeworkViewModel editHomework = null;
            if(homeworkId.HasValue)
            {
                var homework = await dbContext.Homeworks.FindAsync(homeworkId.Value);
                if(homework != null)
                {
                    editHomework = new HomeworkViewModel
                    {
                        homework_id = homework.homework_id,
                        subject_id = homework.subject_id,
                        title = homework.title,
                        description = homework.description,
                        startdate = homework.startdate,
                        enddate = homework.enddate,
                        //attachment = homework.attachment
                    };
                }
            }*/

            ViewData["SubjectId"] = subjectId;

            return View(homeworks);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int subjectId)
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
        public async Task<IActionResult> Create(HomeworkViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? uniqueFilename = null;
                if (model.attachment != null && model.attachment.Length > 0)
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
                return RedirectToAction("Index", "HomeworkHub", new { subjectId = model.subject_id });
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? homeworkid)
        {
            var homework = await dbContext.Homeworks.FirstOrDefaultAsync(h => h.homework_id == homeworkid);
            //Console.WriteLine(homework);
            if (homework == null)
            {
                return NotFound();
            }

            return View(homework);
        }

        [HttpPost]
        public async Task<IActionResult> EditHomework(HomeworkViewModel model)
        {
            if (ModelState.IsValid)
            {
                var homework = await dbContext.Homeworks.FindAsync(model.homework_id);
                if(homework == null)
                {
                    return NotFound();
                }

                homework.subject_id = model.subject_id;
                homework.title = model.title;
                homework.description = model.description;
                homework.startdate = model.startdate;
                homework.enddate = model.enddate;
                //homework.attachment = uniqueFilename;

                //string uniqueFilename = homework.attachment;
                if(model.attachment != null)
                {
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "attachments");
                    string uniqueFilename = Guid.NewGuid().ToString() + "_" + model.attachment.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFilename);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.attachment.CopyToAsync(fileStream);
                    }

                    if(!string.IsNullOrEmpty(homework.attachment))
                    {
                        string oldFilePath = Path.Combine(uploadFolder, homework.attachment);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                        //System.IO.File.Delete(oldFilePath);
                    }
                    homework.attachment = uniqueFilename;
                }

     

                dbContext.Homeworks.Update(homework);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("Index", new { subjectId = homework.subject_id });

            }
            return View(model);
        }

        /*[HttpGet]
        public async Task<IActionResult> Delete(int? homeworkid)
        {
            *//*var homework = await dbContext.Homeworks.FindAsync(homeworkId);
            if (homework == null)
            {
                return NotFound();
            }
            return View(homework);*//*
            var homework = await dbContext.Homeworks.FirstOrDefaultAsync(h => h.homework_id == homeworkid);
            
            if (homework == null)
            {
                return NotFound();
            }

            return View(homework);
        }*/

        [HttpPost]
        public async Task<IActionResult> DeleteHomework(int homeworkid)
        {

            var homework = await dbContext.Homeworks.FirstOrDefaultAsync(h => h.homework_id == homeworkid);
            if (homework == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(homework.attachment))
            {
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "attachments");
                string filePath = Path.Combine(uploadFolder, homework.attachment);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            dbContext.Homeworks.Remove(homework);
            await dbContext.SaveChangesAsync();
            TempData["SuccessMessage"] = "Homework deleted successfully";
            return RedirectToAction("Index", new { subjectId = homework.subject_id });
        }

        

        public IActionResult staff_class()
        {
            return View();
        }

        public IActionResult staff_homework()
        {
            return View();
        }

        public IActionResult staff_submission()
        {
            return View();
        }

    }
}
