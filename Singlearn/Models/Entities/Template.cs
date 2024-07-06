using System.ComponentModel.DataAnnotations;

namespace Singlearn.Models.Entities
{
    public class Template
    {
        [Key]
        public int template_id { get; set; }

        public string layout { get; set; }
    }
}
