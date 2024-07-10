using Microsoft.AspNetCore.Mvc;
using SinglearnWeb.Data;
using Microsoft.EntityFrameworkCore;
using Singlearn.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SinglearnWeb.Controllers
{
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public StaffController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public async Task<IActionResult> Announcement()
        {
            var announcements = await dbContext.Announcement.ToListAsync();
            return View(announcements);
        }

        // GET: Create announcement
        public IActionResult CreateAnnouncement()
        {
            return View();
        }

        // POST: Create announcement
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnnouncement(Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(announcement);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Announcement));
            }
            return View(announcement);
        }

        // GET: Edit announcement
        public async Task<IActionResult> EditAnnouncement(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await dbContext.Announcement.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }
            return View(announcement);
        }

        // POST: Edit announcement
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAnnouncement(int id, Announcement announcement)
        {
            if (id != announcement.announcement_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(announcement);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementExists(announcement.announcement_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Announcement));
            }
            return View(announcement);
        }

        // GET: Delete announcement
        public async Task<IActionResult> DeleteAnnouncement(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await dbContext.Announcement
                .FirstOrDefaultAsync(m => m.announcement_id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // POST: Delete announcement
        [HttpPost, ActionName("DeleteAnnouncement")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var announcement = await dbContext.Announcement.FindAsync(id);
            dbContext.Announcement.Remove(announcement);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Announcement));
        }

        private bool AnnouncementExists(int id)
        {
            return dbContext.Announcement.Any(e => e.announcement_id == id);
        }

        // Other existing actions
        public IActionResult HomeworkHubSubject()
        {
            return View();
        }

        public IActionResult HomeworkHubClass()
        {
            return View();
        }

        public IActionResult HomeworkHubHomework()
        {
            return View();
        }

        public IActionResult HomeworkHubSubmission()
        {
            return View();
        }
    }
}
