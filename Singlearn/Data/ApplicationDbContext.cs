using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Identity.Client;
using Singlearn.Models.Entities;
namespace Singlearn.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<ChapterName> ChapterNames { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<STCTemplate> STCTemplates { get; set; }
        public DbSet<SubjectTeacherClass> SubjectTeacherClasses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Template> Templates { get; set; }

        public DbSet<Homework> Homeworks { get; set; }




    }
}
