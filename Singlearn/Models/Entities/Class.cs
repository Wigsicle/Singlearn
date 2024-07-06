using System.ComponentModel.DataAnnotations;

namespace Singlearn.Models.Entities
{
    public class Class
    {
        [Key]
        public int class_id { get; set; }

        public string name { get; set; }

        public string teacher_id { get; set; }
    }
}
