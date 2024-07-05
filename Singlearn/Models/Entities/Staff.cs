using System.ComponentModel.DataAnnotations;

namespace SinglearnWeb.Models.Entities
{
    public class Staff
    {
        [Key]
        public string staff_id { get; set; }

        public string user_id { get; set; }

        public string name { get; set; }

        public string contact_no { get; set; }
    }
}
