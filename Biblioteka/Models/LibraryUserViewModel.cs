namespace Biblioteka.Models
{
    public class LibraryUserViewModel
    {
        public IEnumerable<LibraryUser> Czytelnicy { get; set; }
        public IEnumerable<Wyporzyczenie> Wyporzyczenia { get; set; }
    }
}
