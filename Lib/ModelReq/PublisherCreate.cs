using System.ComponentModel.DataAnnotations;

namespace Lib.ModelReq
{
    public class PublisherCreate
    {
        [Required]
        public string Name { get; set; }
    }
}
