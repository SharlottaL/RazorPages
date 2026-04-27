using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RazorPages.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Фамилия может быть не более 50ти символов")]
        [Display(Name = "Фамилия")]
        [RegularExpression(@"^[A-ZА-Я]+[a-zа-я]*$", ErrorMessage = "Строка содержит недопустимые символы")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Имя ГДЕ?!")]
        [StringLength(50, ErrorMessage = "Имя может быть не более 50ти символов")]
        [Display(Name = "Имя")]
        [RegularExpression(@"^[A-ZА-Я]+[a-zа-я]*$", ErrorMessage = "Строка содержит недопустимые символы")]
        public string FirstName { get; set; }

        [Display(Name = "Дата зачисления")]
        [DataType(DataType.Date)]
        ///
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Студент")]
        public string FullName
        {
            get => $"{LastName} {FirstName}";
        }

        ////NP/////
        public ICollection<Enrollment> Enrollments { get; set; }

    }
}
