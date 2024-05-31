namespace Lib.Models
{
    public class ReleaseYear
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public List<Book>? Books { get; set; }
    }
}
