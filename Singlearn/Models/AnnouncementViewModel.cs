using System;
using System.ComponentModel.DataAnnotations;

namespace Singlearn.ViewModels
{
	public class AnnouncementViewModel
	{
		[Required]
		public int AnnouncementId { get; set; }

        public int? SubjectId { get; set; }

        public string StaffId { get; set; }

        public string ClassId { get; set; }


        public string Title { get; set; }

		public string Description { get; set; }

		public string Category { get; set; }

		public string Status { get; set; }

		public string Image { get; set; }

		public DateTime Date { get; set; }

		public string StaffName { get; set; }


		// Foreign Key


        // Additional properties for view

        public string Url { get; set; }
	}
}