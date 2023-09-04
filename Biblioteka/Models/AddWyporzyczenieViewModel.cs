namespace Biblioteka.Models
{
    public class AddWyporzyczenieViewModel
    {
        public IEnumerable<LibraryUser>? Czytelnicy { get; set; }
        public IEnumerable<Zasob>? Zasoby { get; set; }
        public int? CzytelnikId { get; set; }
        public int? ZasobId { get; set; }

    }
}
