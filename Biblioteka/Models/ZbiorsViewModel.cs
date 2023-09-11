namespace Biblioteka.Models
{
    public class ZbiorsViewModel
    {
        public IEnumerable<Zbior> Zbiors { get; set; }
        public IEnumerable<Wyporzyczenie> wyporzyczenies { get; set; }
    }
}
