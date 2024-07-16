namespace Singlearn.Models
{
    public class SubmissionViewModel
    {
        public int submission_id {  get; set; }

        public string class_id { get; set; }

        public int homework_id { get; set; }

        public string originalFilename { get; set; }

        public string annotatedFilename { get; set; }

        public string feedback { get; set; }

        public string grade { get; set; }

        public string status { get; set; }

        public Boolean visibility { get; set; }

        public string student_name { get; set; }


    }
}
