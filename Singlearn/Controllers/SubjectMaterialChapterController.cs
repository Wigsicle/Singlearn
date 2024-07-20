using System;
using System.Collections.Generic;
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
    [Route("[controller]/[action]")]
    public class SubjectMaterialChapterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectMaterialChapterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Materials methods
        [HttpGet]
        [Route("/Materials/Index")]
        public async Task<IActionResult> IndexMaterials()
        {
            return View("Materials/Index", await _context.Materials.ToListAsync());
        }

        [HttpGet]
        [Route("/Materials/Details/{id?}")]
        public async Task<IActionResult> DetailsMaterial(int? id)
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

            return View("Materials/Details", material);
        }

        [HttpGet]
        [Route("/Materials/Create")]
        public IActionResult CreateMaterial()
        {
            PopulateViewBag();
            return View("Materials/Create");
        }

        [HttpPost]
        [Route("/Materials/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMaterial(MaterialCreateViewModel model)
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

                if (model.DataFile != null && model.DataFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.DataFile.CopyToAsync(memoryStream);
                        material.data = memoryStream.ToArray();
                        material.file_type = Path.GetExtension(model.DataFile.FileName).Substring(1).ToUpper();
                    }
                }

                if (model.PDFFile != null && model.PDFFile.Length > 0)
                {
                    material.pdf_file = await GetPdfFileBytesAsync(model.PDFFile);
                }

                _context.Add(material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexMaterials));
            }

            PopulateViewBag();
            return View("Materials/Create", model);
        }

        [HttpGet]
        [Route("/Materials/Edit/{id?}")]
        public async Task<IActionResult> EditMaterial(int? id)
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
            };

            PopulateViewBag();
            return View("Materials/Edit", viewModel);
        }

        [HttpPost]
        [Route("/Materials/Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMaterial(int id, MaterialEditViewModel material)
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

                    if (material.DataFile != null && material.DataFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await material.DataFile.CopyToAsync(memoryStream);
                            existingMaterial.data = memoryStream.ToArray();
                        }
                    }

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
                return RedirectToAction(nameof(IndexMaterials));
            }
            return View("Materials/Edit", material);
        }

        // GET: Materials/Delete/5
        [HttpGet]
        [Route("/Materials/Delete/{id?}")]
        public async Task<IActionResult> DeleteMaterial(int? id)
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

            return View("Materials/Delete", material);
        }

        [HttpPost, ActionName("DeleteConfirmedMaterial")]
        [Route("/Materials/DeleteConfirmedMaterial/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material != null)
            {
                _context.Materials.Remove(material);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(IndexMaterials));
        }


        // ChapterNames methods
        [HttpGet]
        [Route("/ChapterNames/Index")]
        public async Task<IActionResult> IndexChapterNames()
        {
            return View("ChapterNames/Index", await _context.ChapterNames.ToListAsync());
        }

        [HttpGet]
        [Route("/ChapterNames/Details/{id?}")]
        public async Task<IActionResult> DetailsChapterName(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapterName = await _context.ChapterNames
                .FirstOrDefaultAsync(m => m.chapter_name_id == id);
            if (chapterName == null)
            {
                return NotFound();
            }
            return View("ChapterNames/Details", chapterName);
        }

        [HttpGet]
        [Route("/ChapterNames/Create")]
        public IActionResult CreateChapterName()
        {
            return View("ChapterNames/Create");
        }

        [HttpPost]
        [Route("/ChapterNames/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChapterName([Bind("chapter_name_id,name,chapter_id,subject_id")] ChapterName chapterName)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chapterName);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexChapterNames));
            }
            return View("ChapterNames/Create", chapterName);
        }

        [HttpGet]
        [Route("/ChapterNames/Edit/{id?}")]
        public async Task<IActionResult> EditChapterName(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapterName = await _context.ChapterNames.FindAsync(id);
            if (chapterName == null)
            {
                return NotFound();
            }
            return View("ChapterNames/Edit", chapterName);
        }

        [HttpPost]
        [Route("/ChapterNames/Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditChapterName(int id, [Bind("chapter_name_id,name,chapter_id,subject_id")] ChapterName chapterName)
        {
            if (id != chapterName.chapter_name_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chapterName);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChapterNameExists(chapterName.chapter_name_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexChapterNames));
            }
            return View("ChapterNames/Edit", chapterName);
        }

        [HttpGet]
        [Route("/ChapterNames/Delete/{id?}")]
        public async Task<IActionResult> DeleteChapterName(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapterName = await _context.ChapterNames
                .FirstOrDefaultAsync(m => m.chapter_name_id == id);
            if (chapterName == null)
            {
                return NotFound();
            }

            return View("ChapterNames/Delete", chapterName);
        }

        [HttpPost, ActionName("DeleteConfirmedChapterName")]
        [Route("/ChapterNames/DeleteConfirmedChapterName/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedChapterName(int id)
        {
            var chapterName = await _context.ChapterNames.FindAsync(id);
            if (chapterName != null)
            {
                _context.ChapterNames.Remove(chapterName);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexChapterNames));
        }

        // Subjects methods
        [HttpGet]
        [Route("/Subjects/Index")]
        public async Task<IActionResult> IndexSubjects()
        {
            return View("Subjects/Index", await _context.Subjects.ToListAsync());
        }

        [HttpGet]
        [Route("/Subjects/Details/{id?}")]
        public async Task<IActionResult> DetailsSubject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.subject_id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View("Subjects/Details", subject);
        }

        [HttpGet]
        [Route("/Subjects/Create")]
        public IActionResult CreateSubject()
        {
            return View("Subjects/Create");
        }

        [HttpPost]
        [Route("/Subjects/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubject([Bind("subject_id,name,year,academic_level,no_chapters,image")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexSubjects));
            }
            return View("Subjects/Create", subject);
        }

        [HttpGet]
        [Route("/Subjects/Edit/{id?}")]
        public async Task<IActionResult> EditSubject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View("Subjects/Edit", subject);
        }

        [HttpPost]
        [Route("/Subjects/Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubject(int id, [Bind("subject_id,name,year,academic_level,no_chapters,image")] Subject subject)
        {
            if (id != subject.subject_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.subject_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexSubjects));
            }
            return View("Subjects/Edit", subject);
        }

        [HttpGet]
        [Route("/Subjects/Delete/{id?}")]
        public async Task<IActionResult> DeleteSubject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.subject_id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View("Subjects/Delete", subject);
        }

        [HttpPost, ActionName("DeleteConfirmedSubject")]
        [Route("/Subjects/DeleteConfirmedSubject/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexSubjects));
        }

        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.material_id == id);
        }

        private bool ChapterNameExists(int id)
        {
            return _context.ChapterNames.Any(e => e.chapter_name_id == id);
        }

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.subject_id == id);
        }

        private async Task<byte[]> GetPdfFileBytesAsync(IFormFile pdfFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await pdfFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
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
        }
}
