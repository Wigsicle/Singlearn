namespace Singlearn.Models
{
    public class UploadHomeworkViewModel
    {

        public int homework_id { get; set; }   

        public int subject_id { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public IFormFile? file { get; set; }
    }
}
