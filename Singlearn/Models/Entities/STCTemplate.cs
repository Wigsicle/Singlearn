using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Singlearn.Models.Entities
{
    [Table("STCTemplates")]
    public class STCTemplate
    {
        [Key]
        public int stc_t_id { get; set; }

        public int stc_id { get; set; }

        public int template_id { get; set; }

        public SubjectTeacherClass SubjectTeacherClass { get; set; }
        public Template Template { get; set; }

    }
}
