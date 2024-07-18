using System;
using System.Collections.Generic;
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
    public class SubjectMaterialChapterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectMaterialChapterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Materials Actions
        [HttpGet("materials/index")]
        public async Task<IActionResult> MaterialsIndex()
        {
            return View("Materials/Index", await _context.Materials.ToListAsync());
        }

        [HttpGet("materials/details/{id}")]
        public async Task<IActionResult> MaterialsDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials.FirstOrDefaultAsync(m => m.material_id == id);

            if (material == null)
            {
                return NotFound();
            }

            return View("Materials/Details", material);
        }

        [HttpGet("materials/create")]
        public IActionResult MaterialsCreate()
        {
            PopulateViewBag();
            return View("Materials/Create", new MaterialCreateViewModel());
        }

        [HttpPost("materials/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MaterialsCreate(MaterialCreateViewModel model)
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
                return RedirectToAction(nameof(MaterialsIndex));
            }

            PopulateViewBag();
            return View("Materials/Create", model);
        }

        [HttpGet("materials/edit/{id}")]
        public async Task<IActionResult> MaterialsEdit(int? id)
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

        [HttpPost("materials/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MaterialsEdit(int id, MaterialEditViewModel material)
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
                return RedirectToAction(nameof(MaterialsIndex));
            }
            return View("Materials/Edit", material);
        }

        [HttpGet("materials/delete/{id}")]
        public async Task<IActionResult> MaterialsDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials.FirstOrDefaultAsync(m => m.material_id == id);
            if (material == null)
            {
                return NotFound();
            }
            return View("Materials/Delete", material);
        }

        [HttpPost("materials/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MaterialsDeleteConfirmed(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MaterialsIndex));
        }

        // ChapterNames Actions
        [HttpGet("chapternames/index")]
        public async Task<IActionResult> ChaptersIndex()
        {
            return View("ChapterNames/Index", await _context.ChapterNames.ToListAsync());
        }

        [HttpGet("chapternames/details/{id}")]
        public async Task<IActionResult> ChaptersDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapterName = await _context.ChapterNames.FirstOrDefaultAsync(m => m.chapter_name_id == id);
            if (chapterName == null)
            {
                return NotFound();
            }
            return View("ChapterNames/Details", chapterName);
        }

        [HttpGet("chapternames/create")]
        public IActionResult ChaptersCreate()
        {
            return View("ChapterNames/Create");
        }

        [HttpPost("chapternames/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChaptersCreate([Bind("chapter_name_id,name,chapter_id,subject_id")] ChapterName chapterName)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chapterName);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ChaptersIndex));
            }
            return View("ChapterNames/Create", chapterName);
        }

        [HttpGet("chapternames/edit/{id}")]
        public async Task<IActionResult> ChaptersEdit(int? id)
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

        [HttpPost("chapternames/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChaptersEdit(int id, [Bind("chapter_name_id,name,chapter_id,subject_id")] ChapterName chapterName)
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
                return RedirectToAction(nameof(ChaptersIndex));
            }
            return View("ChapterNames/Edit", chapterName);
        }

        [HttpGet("chapternames/delete/{id}")]
        public async Task<IActionResult> ChaptersDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chapterName = await _context.ChapterNames.FirstOrDefaultAsync(m => m.chapter_name_id == id);
            if (chapterName == null)
            {
                return NotFound();
            }
            return View("ChapterNames/Delete", chapterName);
        }

        [HttpPost("chapternames/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChaptersDeleteConfirmed(int id)
        {
            var chapterName = await _context.ChapterNames.FindAsync(id);
            _context.ChapterNames.Remove(chapterName);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ChaptersIndex));
        }

        // Subject Actions
        [HttpGet("subjects/index")]
        public async Task<IActionResult> SubjectsIndex()
        {
            return View("Subjects/Index", await _context.Subjects.ToListAsync());
        }

        [HttpGet("subjects/details/{id}")]
        public async Task<IActionResult> SubjectsDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FirstOrDefaultAsync(m => m.subject_id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View("Subjects/Details", subject);
        }

        [HttpGet("subjects/create")]
        public IActionResult SubjectsCreate()
        {
            return View("Subjects/Create");
        }

        [HttpPost("subjects/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubjectsCreate([Bind("subject_id,name,year,academic_level,no_chapters,image")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(SubjectsIndex));
            }
            return View("Subjects/Create", subject);
        }

        [HttpGet("subjects/edit/{id}")]
        public async Task<IActionResult> SubjectsEdit(int? id)
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

        [HttpPost("subjects/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubjectsEdit(int id, [Bind("subject_id,name,year,academic_level,no_chapters,image")] Subject subject)
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
                return RedirectToAction(nameof(SubjectsIndex));
            }
            return View("Subjects/Edit", subject);
        }

        [HttpGet("subjects/delete/{id}")]
        public async Task<IActionResult> SubjectsDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FirstOrDefaultAsync(m => m.subject_id == id);
            if (subject == null)
            {
                return NotFound();
            }
            return View("Subjects/Delete", subject);
        }

        [HttpPost("subjects/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubjectsDeleteConfirmed(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SubjectsIndex));
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
    }
}
