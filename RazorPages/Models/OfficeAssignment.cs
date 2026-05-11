using System.ComponentModel.DataAnnotations;
namespace RazorPages.Models
{
    public class OfficeAssignment
    {
        [Key]
       public int InstructorID { get; set; }
        [StringLength(50)]
        [Display(Name = "Расположение оффиса")]
        public string Location { get; set; }

        //////NP//////
        ///
        public Instructor Instructor { get; set; }
    }
}
