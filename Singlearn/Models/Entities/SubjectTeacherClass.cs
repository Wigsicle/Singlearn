﻿using SinglearnWeb.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Singlearn.Models.Entities
{
    public class SubjectTeacherClass
    {
        [Key]
        public int stc_id { get; set; }

        public int subject_id { get; set; }

        public string teacher_id { get; set; }

        public int class_id { get; set; }

        public Subject Subject { get; set; }
        public Staff Teacher { get; set; }
        public Class Class { get; set; }
    }
}
