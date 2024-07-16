using System.ComponentModel.DataAnnotations;

namespace Singlearn.Models.Entities
{
    public class Submission
    {
        [Key]
        public int submission_id { get; set; }

        public string class_id { get; set; }

        public int homework_id { get; set; }

        public byte[]? originalFilename { get; set; }

        public byte[]? annotatedFilename { get; set; }

        public string feedback {  get; set; }

        public string grade { get; set; }

        public string status { get; set; }

        public Boolean visibility { get; set; }

    }
}
