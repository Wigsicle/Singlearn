using System.ComponentModel.DataAnnotations;

namespace Singlearn.Models.Entities
{
    public class Class
    {
        [Key]
        public string class_Id { get; set; }

        public string name { get; set; }

        public string teacher_id { get; set; }

        public string academic_level { get; set; }
        public int year { get; set; }

        public ICollection<SubjectTeacherClass> SubjectTeacherClasses { get; set; }
    }
}
