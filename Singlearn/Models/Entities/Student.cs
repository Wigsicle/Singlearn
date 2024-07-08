using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Singlearn.Models.Entities
{
    [Table("Students")]
    public class Student
    {
        [Key]
        public string student_id { get; set; }

        public Guid user_id { get; set; }

        public string name { get; set; }

        public string contact_no { get; set; }

        public string class_id { get; set; }
    }
}
