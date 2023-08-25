using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Models
{
    public class Collection
    {
        public int Id { get; set; }
        [Required]
        public Resource Resource { get; set; }
        [Required]
        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }
    }
}
