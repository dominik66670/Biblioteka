namespace Biblioteka.Models
{
    public class AddEditZasobViewModel
    {
        public Zasob Zasob { get; set; }
        public List<Autor> Autorzy { get; set; }
        public AddEditZasobViewModel() 
        {
            Autorzy = new List<Autor>();
        }
    }
}
