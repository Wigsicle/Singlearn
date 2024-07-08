using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Singlearn.Data;
using Singlearn.ViewModels;
using Microsoft.EntityFrameworkCore;
using Singlearn.Models;


namespace Singlearn.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public SubjectController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IActionResult> Subject(int subject_id, string class_id)
        {
            // Set the subject ID and class ID in ViewData for use in the view
            ViewData["SubjectId"] = subject_id;
            ViewData["ClassId"] = class_id;

            var subject_name = await dbContext.Subjects
                .Where(s => s.subject_id.Equals(subject_id))
                .Select(s => s.name)
                .FirstOrDefaultAsync();

            // Query the Materials table
            var materials = await dbContext.Materials
                .Where(m => m.subject_id.Equals(subject_id) && m.class_id == class_id)
                .Select(m => new MaterialViewModel
                {
                    material_id = m.material_id,
                    subject_id = m.subject_id,
                    teacher_id = m.teacher_id,
                    class_id = m.class_id,
                    name = m.name,
                    description = m.description,
                    chapter_id = m.chapter_id,
                    type = m.type,
                    link = m.link,
                    status = m.status
                })
                .ToListAsync();

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

            return View(chapters);
        }
    }
}

