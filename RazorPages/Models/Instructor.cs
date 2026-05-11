using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace RazorPages.Models
{
    public class Instructor
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-ZА-Я]+[a-zа-я]*$",ErrorMessage = "Строка содержит недопустимые символы")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-ZА-Я]+[a-zа-я]*$", ErrorMessage = "Строка содержит недопустимые символы")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата трудоустройства")]
        public DateTime HireDate { get; set; }
        [Display(Name = "Инструктор")]
        public string FullName
        {
            get => $"{LastName}, {FirstName}";
        }

        ///////NP////////
        ///
        public ICollection<Course> Courses { get; set; }
        public OfficeAssignment OfficeAssignment { get; set; }
    }
}
