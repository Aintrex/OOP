using System.ComponentModel.DataAnnotations;

namespace Lib.ModelReq
{
    public class ReleaseYearCreate
    {
        [Required]
        public int Year { get; set; }
    }
}
