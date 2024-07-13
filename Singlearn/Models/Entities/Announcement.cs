using System;
using System.ComponentModel.DataAnnotations;

namespace Singlearn.Models.Entities
{
    public class Announcement
    {
        [Key]
        public int announcement_id { get; set; }
        public int? subject_id { get; set; }
        public string staff_id { get; set; }
        public string? class_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public DateTime date { get; set; }
        /*public string message_body { get; set; }*/
        public string? url { get; set; }
        public string category { get; set; }
        public string status { get; set; }

    }
}
