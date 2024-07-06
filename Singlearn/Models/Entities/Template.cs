using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Singlearn.Models.Entities
{
    [Table("Templates")]
    public class Template
    {
        [Key]
        public int template_id { get; set; }

        public string name { get; set; }

        public string view_name { get; set; }
    }
}
