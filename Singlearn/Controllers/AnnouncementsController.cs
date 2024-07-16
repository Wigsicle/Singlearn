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
                Status = announcement.status,
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
            var model = new AnnouncementViewModel
            {
                Date = DateTime.Now
            };
            return View(model);
        }


        // POST: Announcements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnnouncementViewModel model)
        {
            if (ModelState.IsValid)
            {
                var announcement = new Announcement
                {
                    title = model.Title,
                    description = model.Description,
                    category = model.Category,
                    status = model.Status,
                    image = model.Image,
                    date = model.Date,
                    url = model.Url,
                    subject_id = model.SubjectId,
                    staff_id = model.StaffId,
                    class_id = model.ClassId
                };

                _context.Announcements.Add(announcement);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Announcements/Edit
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
                Status = announcement.status,
                Image = announcement.image,
                Date = announcement.date,
                SubjectId = announcement.subject_id,
                StaffId = announcement.staff_id,
                ClassId = announcement.class_id,
                Url = announcement.url
            };

            // Log details for debugging
            Console.WriteLine($"GET Edit: Found announcement with ID {id}");
            Console.WriteLine($"Title: {announcement.title}, Description: {announcement.description}");

            return View(model);
        }

        // POST: Announcements/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AnnouncementViewModel model)
        {

            try
            {
                var announcement = await _context.Announcements.FindAsync(id);
                if (announcement == null)
                {
                    Console.WriteLine($"Announcement with ID {id} not found in the database.");
                    return NotFound();
                }

                // Log the current state of the announcement
                Console.WriteLine($"POST Edit: Editing announcement with ID {id}");
                Console.WriteLine($"Old Title: {announcement.title}, New Title: {model.Title}");

                announcement.title = model.Title;
                announcement.description = model.Description;
                announcement.category = model.Category;
                announcement.status = model.Status;
                announcement.image = model.Image;
                announcement.date = model.Date;
                announcement.subject_id = model.SubjectId;
                announcement.staff_id = model.StaffId;
                announcement.class_id = model.ClassId;
                announcement.url = model.Url;

                _context.Update(announcement);
                await _context.SaveChangesAsync();

                Console.WriteLine("Announcement successfully updated.");
                return RedirectToAction("Index", "Announcements");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnouncementExists(model.AnnouncementId))
                {
                    Console.WriteLine($"Concurrency error: Announcement with ID {model.AnnouncementId} no longer exists.");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            // Log validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            foreach (var error in errors)
            {
                Console.WriteLine($"Validation error: {error}");
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
                Status = announcement.status,
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
