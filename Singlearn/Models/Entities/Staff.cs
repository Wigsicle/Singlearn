using System.ComponentModel.DataAnnotations;

namespace Singlearn.Models.Entities
{
    public class Staff
    {
        [Key]
        public string staff_id { get; set; }

        public Guid user_id { get; set; }

        public string name { get; set; }

        public string contact_no { get; set; }

        public ICollection<SubjectTeacherClass> SubjectTeacherClasses { get; set; }
    }
}
