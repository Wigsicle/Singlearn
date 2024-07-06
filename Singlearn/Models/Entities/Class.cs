using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Singlearn.Models.Entities
{
    [Table("Classes")]
    public class Class
    {
        [Key]
        public int class_id { get; set; }

        public string name { get; set; }

        public string teacher_id { get; set; }

        public ICollection<SubjectTeacherClass> SubjectTeacherClasses { get; set; }
    }
}
