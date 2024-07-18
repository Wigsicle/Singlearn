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
    public class STCTemplatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public STCTemplatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: STCTemplates
        public async Task<IActionResult> Index()
        {
            return View(await _context.STCTemplates.ToListAsync());
        }

        // GET: STCTemplates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stctemplates = await _context.STCTemplates
                .FirstOrDefaultAsync(m => m.stc_id == id);
            if (stctemplates == null)
            {
                return NotFound();
            }

            return View(stctemplates);
        }

        // GET: STCTemplates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: STCTemplates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("stc_t_id,stc_id,template_id")] STCTemplate stcTemplate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stcTemplate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stcTemplate);
        }

        // GET: STCTemplates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stcTemplate = await _context.STCTemplates.FindAsync(id);
            if (stcTemplate == null)
            {
                return NotFound();
            }
            return View(stcTemplate);
        }

        // POST: STCTemplates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("stc_t_id,stc_id,template_id")] STCTemplate stcTemplate)
        {
            if (id != stcTemplate.stc_t_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stcTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!STCTemplateExists(stcTemplate.stc_t_id))
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
            return View(stcTemplate);
        }

        // GET: STCTemplates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stcTemplate = await _context.STCTemplates
                .FirstOrDefaultAsync(m => m.stc_id == id);
            if (stcTemplate == null)
            {
                return NotFound();
            }

            return View(stcTemplate);
        }

        // POST: STCTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stcTemplate = await _context.STCTemplates.FindAsync(id);
            if (stcTemplate != null)
            {
                _context.STCTemplates.Remove(stcTemplate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool STCTemplateExists(int id)
        {
            return _context.STCTemplates.Any(e => e.stc_id == id);
        }
    }
}
