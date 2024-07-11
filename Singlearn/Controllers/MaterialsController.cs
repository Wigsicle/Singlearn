using System.IO;
using System.Linq.Expressions;
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
                if (model.DataFile != null && model.DataFile.Length > 0 && model.PDFFile != null && model.PDFFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.DataFile.CopyToAsync(memoryStream);
                        await model.PDFFile.CopyToAsync(memoryStream);
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
                            data = memoryStream.ToArray(),
                            file_type = model.file_type,
                            pdf_file = memoryStream.ToArray()
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
            try
            {
                var material = _context.Materials.Find(id);

                if (material == null || material.data == null || material.data.Length == 0)
                {
                    return NotFound();
                }

                string contentType;
                string fileExtension;

                switch (material.file_type)
                {
                    case "DOCX":
                        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        fileExtension = ".docx";
                        break;
                    case "PPTX":
                        contentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                        fileExtension = ".pptx";
                        break;
                    case "XLSX":
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        fileExtension = ".xlsx";
                        break;
                    default:
                        contentType = "application/pdf";
                        fileExtension = ".pdf";
                        break;
                }

                return File(material.data, contentType, material.name + fileExtension);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Handle the exception (e.g., return a 500 error)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        // Other actions like Index, Edit, Delete, etc.
    }
}
