using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Singlearn.Models.Entities
{
    public class STCTemplate
    {
        [Key]
        public int stc_t_id { get; set; }

        public int stc_id { get; set; }

        public int template_id { get; set; }


    }
}
