using System.ComponentModel.DataAnnotations;

namespace Lib.ModelReq
{
    public class CountryCreate
    {
        [Required]
        public string Name { get; set; }
    }
}
