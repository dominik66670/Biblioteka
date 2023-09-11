using X.PagedList;

namespace Biblioteka.Models
{
    public class AutorsViewModel
    {
        public IPagedList<Autor> autors { get; set; }
        public IEnumerable<Zasob> zasobs { get; set; }
    }
}
