using System.ComponentModel.DataAnnotations;

namespace Singlearn.Models.Entities
{
    public class Homework
    {
        [Key]
        public int homework_id { get; set; }

        public int subject_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }

        public byte[]? attachment { get; set; }

        public string attachmentName { get; set; }

        public DateTime startdate { get; set; }

        public DateTime enddate { get; set; }
    }
}
