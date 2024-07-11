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
    public class AnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Announcement()
        {
            return View();
        }

        // GET: Announcements
        public async Task<IActionResult> Index()
        {
            return View(await _context.Announcements.ToListAsync());
        }

        // GET: Announcements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements
                .FirstOrDefaultAsync(m => m.announcement_id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            var model = new AnnouncementViewModel
            {
                AnnouncementId = announcement.announcement_id,
                Title = announcement.title,
                Description = announcement.description,
                Category = announcement.category,
                Status = announcement.Status,
                Image = announcement.image,
                Date = announcement.date,
                Url = announcement.url,
                SubjectId = announcement.subject_id,
                StaffId = announcement.staff_id,
                ClassId = announcement.class_id
            };

            return View(model);
        }

        // GET: Announcements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Announcements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AnnouncementViewModel model)
        {
            if (ModelState.IsValid)
            {
                var announcement = new Announcement
                {
                    title = model.Title,
                    description = model.Description,
                    category = model.Category,
                    Status = model.Status,
                    image = model.Image,
                    date = model.Date,
                    url = model.Url,
                    subject_id = model.SubjectId,
                    staff_id = model.StaffId,
                    class_id = model.ClassId
                };

                _context.Announcements.Add(announcement);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Announcements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            var model = new AnnouncementViewModel
            {
                AnnouncementId = announcement.announcement_id,
                Title = announcement.title,
                Description = announcement.description,
                Category = announcement.category,
                Status = announcement.Status,
                Image = announcement.image,
                Date = announcement.date,
                Url = announcement.url,
                SubjectId = announcement.subject_id,
                StaffId = announcement.staff_id,
                ClassId = announcement.class_id,
            };

            return View(model);
        }

        // POST: Announcements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AnnouncementViewModel model)
        {
            if (id != model.AnnouncementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var announcement = await _context.Announcements.FindAsync(id);
                    if (announcement == null)
                    {
                        return NotFound();
                    }

                    announcement.title = model.Title;
                    announcement.description = model.Description;
                    announcement.category = model.Category;
                    announcement.Status = model.Status;
                    announcement.image = model.Image;
                    announcement.date = model.Date;
                    announcement.url = model.Url;
                    announcement.subject_id = model.SubjectId;
                    announcement.staff_id = model.StaffId;
                    announcement.class_id = model.ClassId;

                    _context.Update(announcement);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementExists(model.AnnouncementId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }

            return View(model);
        }

        // GET: Announcements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements
                .FirstOrDefaultAsync(m => m.announcement_id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            var model = new AnnouncementViewModel
            {
                AnnouncementId = announcement.announcement_id,
                Title = announcement.title,
                Description = announcement.description,
                Category = announcement.category,
                Status = announcement.Status,
                Image = announcement.image,
                Date = announcement.date,
                Url = announcement.url,
                SubjectId = announcement.subject_id,
                StaffId = announcement.staff_id,
                ClassId = announcement.class_id
            };

            return View(model);
        }

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement != null)
            {
                _context.Announcements.Remove(announcement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnouncementExists(int id)
        {
            return _context.Announcements.Any(e => e.announcement_id == id);
        }
    }
}
