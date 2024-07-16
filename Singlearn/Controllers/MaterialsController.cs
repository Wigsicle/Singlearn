using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                var materials = await _context.Materials.ToListAsync();
                return View(materials);
        }

        // GET: Materials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                .FirstOrDefaultAsync(m => m.material_id == id);

            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        private void PopulateViewBag()
        {
            var typeOptions = new List<SelectListItem>
            {
                new SelectListItem{ Value = "Video Lessons", Text = "Video Lessons" },
                new SelectListItem{ Value = "Lesson Notes", Text = "Lesson Notes"},
                new SelectListItem{ Value = "Classwork", Text = "Classwork"},
                new SelectListItem{ Value = "Homework", Text = "Homework"}
            };
            var statusOptions = new List<SelectListItem>
            {
              new SelectListItem{ Value = "Visible", Text = "Visible" },
              new SelectListItem{ Value = "Not Visible", Text = "Not Visible" }
             };

            ViewBag.TypeOptions = typeOptions;
            ViewBag.StatusOptions = statusOptions;

        }

        // GET: Materials/Create
        public IActionResult Create()
        {
            PopulateViewBag();
            return View(new MaterialCreateViewModel());
        }

        // POST: Materials/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
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
                    file_type = model.file_type
                };

                // Process uploaded data file if present
                if (model.DataFile != null && model.DataFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.DataFile.CopyToAsync(memoryStream);
                        material.data = memoryStream.ToArray();
                        material.file_type = Path.GetExtension(model.DataFile.FileName).Substring(1).ToUpper();
                    }
                }

                // Process uploaded PDF file if present
                if (model.PDFFile != null && model.PDFFile.Length > 0)
                {
                    material.pdf_file = await GetPdfFileBytesAsync(model.PDFFile);
                }

                _context.Add(material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we reach here, something went wrong, so return to the view with the model
            PopulateViewBag();
            return View(model);
        }

        // GET: Materials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            var viewModel = new MaterialEditViewModel
            {
                material_id = material.material_id,
                subject_id = material.subject_id,
                teacher_id = material.teacher_id,
                class_id = material.class_id,
                name = material.name,
                description = material.description,
                chapter_id = material.chapter_id,
                type = material.type,
                link = material.link,
                status = material.status
                // You don't need to set file_type, DataFile, or PDFFile here
            };

            PopulateViewBag();
            return View(viewModel);
        }




        // POST: Materials/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaterialEditViewModel material)
        {
            if (id != material.material_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingMaterial = await _context.Materials.FindAsync(id);
                    if (existingMaterial == null)
                    {
                        return NotFound();
                    }

                    // Update non-file properties
                    existingMaterial.subject_id = material.subject_id;
                    existingMaterial.teacher_id = material.teacher_id;
                    existingMaterial.class_id = material.class_id;
                    existingMaterial.name = material.name;
                    existingMaterial.description = material.description;
                    existingMaterial.chapter_id = material.chapter_id;
                    existingMaterial.type = material.type;
                    existingMaterial.link = material.link;
                    existingMaterial.status = material.status;
                    existingMaterial.file_type = material.file_type;

                    // Process updated data file if present
                    if (material.DataFile != null && material.DataFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await material.DataFile.CopyToAsync(memoryStream);
                            existingMaterial.data = memoryStream.ToArray();
                        }
                    }

                    // Update PDF file if provided
                    if (material.PDFFile != null && material.PDFFile.Length > 0)
                    {
                        existingMaterial.pdf_file = await GetPdfFileBytesAsync(material.PDFFile);
                    }

                    _context.Update(existingMaterial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }


        // GET: Materials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                .FirstOrDefaultAsync(m => m.material_id == id);
            if (material == null)
            {
                return NotFound();
            }

            PopulateViewBag();
            return View(material);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Materials/DownloadDocument/5
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

        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.material_id == id);
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
