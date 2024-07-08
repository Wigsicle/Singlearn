using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Identity.Client;
using Singlearn.Models.Entities;
using SinglearnWeb.Models.Entities;
namespace SinglearnWeb.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<ChapterName> ChapterNames { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<SubjectTeacherClass> SubjectTeacherClasses { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<STCTemplate> STCTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Staff>().ToTable("Staff");
            modelBuilder.Entity<Subject>().ToTable("Subjects");
            modelBuilder.Entity<Class>().ToTable("Classes");
            modelBuilder.Entity<SubjectTeacherClass>().ToTable("SubjectTeacherClasses");
            modelBuilder.Entity<Template>().ToTable("Templates");
            modelBuilder.Entity<STCTemplate>().ToTable("STCTemplates");

            modelBuilder.Entity<SubjectTeacherClass>()
                .HasOne(stc => stc.Subject)
                .WithMany(s => s.SubjectTeacherClasses)
                .HasForeignKey(stc => stc.subject_id);

            modelBuilder.Entity<SubjectTeacherClass>()
                .HasOne(stc => stc.Class)
                .WithMany(c => c.SubjectTeacherClasses)
                .HasForeignKey(stc => stc.class_id);

            modelBuilder.Entity<SubjectTeacherClass>()
                .HasOne(stc => stc.Staff)
                .WithMany(t => t.SubjectTeacherClasses)
                .HasForeignKey(stc => stc.teacher_id);

        }





    }
}
