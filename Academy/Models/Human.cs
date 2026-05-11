using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academy.Models
{
    public class Human
    {
        public string last_name { get; set; }
        [Required]
        public string first_name { get; set; }
        public string? middle_name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly birth_date { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        [Column(TypeName = "image")]
        public byte[]? photo { get; set; }
        public int Age
        {
            get => (int)((DateOnly.FromDateTime(DateTime.Now).DayNumber - birth_date.DayNumber) / 365.25);
        }

    }
}
