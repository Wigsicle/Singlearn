using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Singlearn.Data;
using Singlearn.Models.Entities;

namespace Singlearn.Controllers
{
    public class ChapterNamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChapterNamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChapterNames
        public async Task<IActionResult> Index()
        {
            return View(await _context.ChapterNames.ToListAsync());
        }

        // GET: ChapterNames/Details/5
        public async Task<IActionResult> Details(int? id)
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
            return View(chapterName);
        }

        // GET: ChapterNames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChapterNames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("chapter_name_id,name,chapter_id,subject_id")] ChapterName chapterName)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chapterName);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chapterName);
        }

        // GET: ChapterNames/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            return View(chapterName);
        }

        // POST: ChapterNames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("chapter_name_id,name,chapter_id,subject_id")] ChapterName chapterName)
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
                return RedirectToAction(nameof(Index));
            }
            return View(chapterName);
        }

        // GET: ChapterNames/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(chapterName);
        }

        // POST: ChapterNames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chapterName = await _context.ChapterNames.FindAsync(id);
            if (chapterName != null)
            {
                _context.ChapterNames.Remove(chapterName);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChapterNameExists(int id)
        {
            return _context.ChapterNames.Any(e => e.chapter_name_id == id);
        }
    }
}
