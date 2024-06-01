using System.ComponentModel.DataAnnotations;

namespace Lib.ModelReq
{
    public class LanguageCreate
    {
        [Required]
        public string Name { get; set; }
    }
}
