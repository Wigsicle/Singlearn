using System.ComponentModel.DataAnnotations;

namespace Singlearn.Models.Entities
{
    public class ChapterName
    {
        [Key]
        public int chapter_name_id { get; set; }

        public string name { get; set; }

        public int chapter_id { get; set; }

        public int subject_id { get; set; }
    }
}
