namespace Singlearn.ViewModels
{
    public class SubjectViewModel
    {
        public int subject_id { get; set; }
        public string name { get; set; }
        public string academic_level { get; set; }
        public string image { get; set; }
        public int no_chapters { get; set; }
        public int year { get; set; }
        public string class_id { get; set; }

        public string class_name { get; set; }

        public Singlearn.Models.Entities.SubjectTeacherClass SubjectTeacherClass { get; set; }
        public List<Singlearn.Models.Entities.Announcement> Announcements { get; set; }
        public List<Singlearn.Models.Entities.Material> Materials { get; set; }
        public List<Singlearn.ViewModels.ChapterViewModel> Chapters { get; set; }
        public string TemplateViewName { get; set; }
    }
}