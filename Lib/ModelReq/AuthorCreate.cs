using System.ComponentModel.DataAnnotations;

namespace Lib.ModelReq
{
    public class AuthorCreate
    {
        [Required]
        public string Name { get; set; }
    }
}
