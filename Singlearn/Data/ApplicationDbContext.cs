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

        public DbSet<User> Users { get; set; }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<SubjectTeacherClass> SubjectTeacherClasses { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<STCTemplate> STCTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SubjectTeacherClass>()
                .HasOne(stc => stc.Subject)
                .WithMany()
                .HasForeignKey(stc => stc.subject_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubjectTeacherClass>()
                .HasOne(stc => stc.Class)
                .WithMany()
                .HasForeignKey(stc => stc.class_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubjectTeacherClass>()
                .HasOne(stc => stc.Staff)
                .WithMany()
                .HasForeignKey(stc => stc.teacher_id)
                .OnDelete(DeleteBehavior.Restrict);

            // Define primary keys if not done via data annotations
            modelBuilder.Entity<Subject>()
                .HasKey(s => s.subject_id);

            modelBuilder.Entity<Class>()
                .HasKey(c => c.class_id);

            modelBuilder.Entity<Staff>()
                .HasKey(t => t.staff_id);

            // Define other entity configurations here
            
        }





    }
}
