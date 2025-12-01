using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{

  
        public class ExpenseMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseMasterId { get; set; } // Primary key

       
        public int ExpenseTypeId { get; set; } // Primary key
        public decimal Amount { get; set; } // Primary key

        public String ExpenseVoucher { get; set; } = "";
        public String Remark { get; set; } = "";

        public int RelatedEntityId { get; set; } = 0;
        public String RelatedEntityType { get; set; } = "";
        public String PaymentMode { get; set; } = "";

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; } = "";
        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public int CompanyId { get; set; } = 0;

    }


}
