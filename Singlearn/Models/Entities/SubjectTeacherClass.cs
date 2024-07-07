using SinglearnWeb.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Singlearn.Models.Entities
{
    [Table("SubjectTeacherClasses")]
    public class SubjectTeacherClass
    {
        [Key]
        public int stc_id { get; set; }

        public int subject_id { get; set; }

        public string teacher_id { get; set; }

        public string class_id { get; set; }

        public Subject Subject { get; set; }
        public Staff Staff { get; set; }
        public Class Class { get; set; }
    }
}
