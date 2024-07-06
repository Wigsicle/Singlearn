using System.ComponentModel.DataAnnotations;

namespace Singlearn.Models.Entities
{
    public class StudentTeacherClass
    {
        [Key]
        public int stc_id { get; set; }

        public int subject_id { get; set; }

        public string teacher_id { get; set; }

        public int class_id { get; set; }
    }
}
