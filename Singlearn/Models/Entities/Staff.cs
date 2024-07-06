using Singlearn.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinglearnWeb.Models.Entities
{
    [Table("Staff")]
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
