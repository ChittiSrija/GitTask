using System.ComponentModel.DataAnnotations;

namespace EVOKETASK.Models
{
    public class GENDER
    {
        [Key]
        public int GID { get; set; }
        public string GNAME { get; set; }
    }
}
