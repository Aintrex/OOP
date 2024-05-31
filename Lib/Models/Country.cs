namespace Lib.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Book>? Books { get; set; }
    }
}
