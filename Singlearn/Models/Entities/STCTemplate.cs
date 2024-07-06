using System.ComponentModel.DataAnnotations;

namespace Singlearn.Models.Entities
{
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
