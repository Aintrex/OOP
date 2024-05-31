namespace Lib.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public Author? author { get; set; }
        public int YearId { get; set; }
        public ReleaseYear? ReleaseYear { get; set; }
        public int CountryId { get; set; }
        public Country? country { get; set; }
        public int PublisherId { get; set; }
        public Publisher? publisher { get; set; }
        public int LanguageId { get; set; }
        public Language? language { get; set; }
        public Genre? Genre { get; set; }
        public int GenreId { get; set; }

    }
}
