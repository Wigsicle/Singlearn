using System.ComponentModel.DataAnnotations;

namespace Singlearn.Models.Entities
{
    public class Material
    {
        [Key]
        public int material_id { get; set; }
        public int subject_id { get; set; }
        public string teacher_id { get; set; }
        public string class_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int chapter_id { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public string status { get; set; }
        public byte[] data { get; set; } // For storing varbinary(max) in the database
        public string file_type { get; set; }
        public byte[] pdf_file { get; set; }
    }
}
