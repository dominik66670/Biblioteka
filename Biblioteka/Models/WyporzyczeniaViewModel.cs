namespace Biblioteka.Models
{
    public class WyporzyczeniaViewModel
    {
        public IEnumerable<Wyporzyczenie> wyporzyczenies { get; set; }
        public IEnumerable<Status> statuses { get; set; }
    }
}
