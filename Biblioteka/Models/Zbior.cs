using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Models
{
    public class Zbior
    {
        public int Id { get; set; }
        [Required]
        public Zasob Zasob { get; set; }
        [Required]
        [Display(Name = "Czy dostępny")]
        public bool CzyDostepny { get; set; }
    }
}
