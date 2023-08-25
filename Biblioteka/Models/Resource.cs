using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Models
{
    public class Resource
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ISBN { get; set; }
        public List<Author>? Authors { get; set; }
    }
}
