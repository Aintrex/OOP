using Lib.Models;

namespace Lib.ModelReq
{
    public class BookCreate
    {
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int YearId { get; set; }
        public int CountryId { get; set; }
        public int PublisherId { get; set; }
        public int LanguageId { get; set; }
        public int GenreId { get; set; }
    }
}
