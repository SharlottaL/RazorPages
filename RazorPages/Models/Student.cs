using System.ComponentModel.DataAnnotations;
namespace RazorPages.Models
{
    public class Student
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        ////NP/////
        public ICollection<Enrollment> Enrollments { get; set; }

    }
}
