using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Models
{
    public class Wyporzyczenie
    {
        public int Id { get; set; }
        [Required]
        public LibraryUser Wyporzyczajacy { get; set; }
        [Required]
        [Display(Name = "Wyporzyczany Zbiór")]
        public Zbior WyporzyczanyZbior { get; set; }
        [Display(Name = "Obecny Status")]
        public Status ObecnyStatus { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data Wyporzyczenia")]
        public DateTime DataWyporzyczenia { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data zwrotu")]
        public DateTime DataZwrotu { get; set; }
    }
}
