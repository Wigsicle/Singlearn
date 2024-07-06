using System.ComponentModel.DataAnnotations;
namespace Singlearn.Models.Entities
{
    
    public class Subject
    {
        [Key]
        public int subject_id { get; set; }
        public string name { get; set; }

        public int year { get; set; }

        public string academic_level { get; set; }

        public int no_chapters { get; set; }

        public string image {  get; set; }
    }
}
