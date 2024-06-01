using System.ComponentModel.DataAnnotations;

namespace Lib.ModelReq
{
    public class GenreCreate
    { 
        [Required]
        public string Name { get; set; }
    }
}
