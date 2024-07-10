using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Singlearn.Data;
using Singlearn.Models.Entities;
using Singlearn.ViewModels;

namespace Singlearn.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaterialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Materials
        public async Task<IActionResult> Index()
        {
            return View(await _context.Materials.ToListAsync());
        }

        // GET: Materials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Materials/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Process uploaded file if present
                if (model.DataFile != null && model.DataFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.DataFile.CopyToAsync(memoryStream);
                        var material = new Material
                        {
                            subject_id = model.subject_id,
                            teacher_id = model.teacher_id,
                            class_id = model.class_id,
                            name = model.name,
                            description = model.description,
                            chapter_id = model.chapter_id,
                            type = model.type,
                            link = model.link,
                            status = model.status,
                            data = memoryStream.ToArray()
                        };

                        _context.Add(material);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    ModelState.AddModelError("DataFile", "Please upload a file.");
                }
            }

            // If we reach here, something went wrong, so return to the view with the model
            return View(model);
        }

        public IActionResult DownloadDocument(int id)
        {
            var material = _context.Materials.Find(id);
            if (material == null || material.data == null || material.data.Length == 0)
            {
                return NotFound();
            }

            // Return the document as a FileResult
            return File(material.data, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", material.name + ".docx");
        }


        // Other actions like Index, Edit, Delete, etc.
    }
}
