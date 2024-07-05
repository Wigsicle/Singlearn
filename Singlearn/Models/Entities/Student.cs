using System.ComponentModel.DataAnnotations;

namespace SinglearnWeb.Models.Entities
{
    public class Student
    {
        [Key]
        public string student_id { get; set; }

        public string user_id { get; set; }

        public string name { get; set; }

        public string contact_no { get; set; }

        public string class_id { get; set; }
    }
}
