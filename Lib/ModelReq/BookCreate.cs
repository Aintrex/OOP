using Lib.Models;

namespace Lib.ModelReq
{
    public class BookCreate
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int? Year { get; set; }
        public string Country { get; set; }
        public string Publisher { get; set; }
        public string Language { get; set; }
        public string Genre { get; set; }
    }
}
