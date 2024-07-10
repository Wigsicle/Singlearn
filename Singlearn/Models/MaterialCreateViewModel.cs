﻿using Microsoft.AspNetCore.Http;

namespace Singlearn.ViewModels
{
    public class MaterialCreateViewModel
    {
        public int subject_id { get; set; }
        public string teacher_id { get; set; }
        public string class_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int chapter_id { get; set; }
        public string type { get; set; }
        public string link { get; set; }
        public string status { get; set; }
        public IFormFile DataFile { get; set; } // For file upload
    }
}

