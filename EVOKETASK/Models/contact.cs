using System.ComponentModel.DataAnnotations;

namespace EVOKETASK.Models
{
    public class contact
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Int64 Phone { get; set; }
        public string Message  { get; set; }
    }
}
