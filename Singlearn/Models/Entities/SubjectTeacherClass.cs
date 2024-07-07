using System.ComponentModel.DataAnnotations;

namespace Singlearn.Models.Entities
{
    public class SubjectTeacherClass
    {
        [Key]
        public int stc_id { get; set; }

        public int subject_id { get; set; }

        public string teacher_id { get; set; }

        public string class_id { get; set; }
    }
}
