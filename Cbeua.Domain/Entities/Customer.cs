using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; } // Primary key

        public string CustomerName { get; set; } = "";
        public  string CustomerPhone { get; set; } = "";
        public  string CustomerEmail { get; set; } = "";
        public  string CustomerAddress { get; set; } = "";

        public DateTime DOB { get; set; }
      
        public required string Nationalilty { get; set; } = "";
        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; } = true;
        public int CompanyId { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
    }

   



}
