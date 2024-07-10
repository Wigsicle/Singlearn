using System.ComponentModel.DataAnnotations;

namespace Singlearn.Models.Entities
{
    public class Announcement
    {
        [Key]
        public int announcement_id { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public string category { get; set; }

        public string Status { get; set; }

        public string image { get; set; }

        public DateTime date { get; set; }

        public string url { get; set; }

        // Foreign Key
        public string subject_id { get; set; }

        public string teacher_id { get; set; }

        public string class_id { get; set; }

        public string SubjectType { get; set; }

    }
}
