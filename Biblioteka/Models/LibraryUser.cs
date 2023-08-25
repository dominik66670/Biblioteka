using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Models
{
    public class LibraryUser : IdentityUser
    {
        [Required]
        public string Imie { get; set; }
        [Required]
        public string Nazwisko { get; set; }
        [Required]
        public string Adres { get; set; }
    }
}
