using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public partial class ExpenseMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseMasterId { get; set; }

        public int ExpenseTypeId { get; set; }

        public DateTime ExpenseDate { get; set; }

        public decimal Amount { get; set; }

        public string PaidTo { get; set; } = "";

        public string ReferenceNo { get; set; } = "";

        public string PaymentMode { get; set; } = "";

        public string Description { get; set; } = "";

        public bool IsDeleted { get; set; } = false;
        public Boolean isApproved { get; set; } = false;
        public String? ApprovedBy { get; set; } = "";
        public DateTime? ApprovedDate { get; set; }
    }
}
