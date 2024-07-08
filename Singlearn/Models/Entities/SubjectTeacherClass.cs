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

        [ForeignKey("subject_id")]
        public Subject Subject { get; set; }

        [ForeignKey("teacher_id")]
        public Staff Staff { get; set; }

        [ForeignKey("class_id")]
        public Class Class { get; set; }
    }
}
