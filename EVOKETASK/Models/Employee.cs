using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace EVOKETASK.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public Int64 PHONENUMBER { get; set; }
        public int DEPTID { get; set;}
        public int GenderID { get; set; }
        public string DEPTNAME { get; set; }
    }
}
