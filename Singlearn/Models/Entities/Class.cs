using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Singlearn.Models.Entities
{
    [Table("Classes")]
    public class Class
    {
        [Key]
        public string class_id { get; set; }

        public string name { get; set; }

        public string teacher_id { get; set; }

        public string academic_level { get; set; }

        public int year {  get; set; }

        public ICollection<SubjectTeacherClass> SubjectTeacherClasses { get; set; }
    }
}
