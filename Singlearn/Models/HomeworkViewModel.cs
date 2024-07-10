namespace Singlearn.ViewModels
{
    public class HomeworkViewModel
    {
        public int homework_id { get; set; }
        public int subject_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }

        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }

        public IFormFile attachment { get; set; }
    }
}
