using System.ComponentModel.DataAnnotations;

namespace SinglearnWeb.Models.Entities
{
    public class User
    {
        [Key]
        public Guid user_id { get; set; }
        public string email { get; set; }

        public string password { get; set; }

        public string role { get; set; }
    }
}
