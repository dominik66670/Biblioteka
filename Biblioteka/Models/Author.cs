using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Biblioteka.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date of brith")]
        public DateTime? DateOfBrith { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date of death")]
        public DateTime? DateOfDeath { get; set;}
    }
}
