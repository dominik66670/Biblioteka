using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Biblioteka.Models
{
    public class Zasob
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tytuł")]
        public string Tytul { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Display(Name = "Autorzy")]
        
        public List<Autor>? Autorzy { get; set; }
    }
}
