namespace Lib.ModelReq
{
    public class BookUpdate
    {
        public string ExistingTitle { get; set; }
        public string NewTitle { get; set; }
        public string NewAuthor { get; set; }
        public int? NewYear { get; set; }
        public string NewCountry { get; set; }
        public string NewPublisher { get; set; }
        public string NewLanguage { get; set; }
        public string NewGenre { get; set; }
    }
}
