using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Biblioteka.Models
{
    public class Autor
    {
        public int Id { get; set; }
        [Required]
        public string Nazwa { get; set; }
        [Display(Name = "Opis")]
        public string? Description { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data urodzenia")]
        public DateTime? DataUrodzenia { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date śmierci")]
        public DateTime? DataSmierci { get; set;}
    }
}
