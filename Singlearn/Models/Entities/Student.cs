namespace SinglearnWeb.Models.Entities
{
    public class Student
    {
        public string studentId { get; set; }

        public string userId { get; set; }

        public string name { get; set; }

        public string contactNo { get; set; }

        public DateOnly dob { get; set; }

        public string gender { get; set; }

        public string address { get; set; }

        public string parentName { get; set; }

        public string parentNo { get; set; }
    }
}
