using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace Cbeua.Domain.Entities
{
    public class ExpenseType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseTypeId { get; set; } // Primary key
        public String ExpenseTypeCode { get; set; } = "";
        public String ExpenseTypeName { get; set; } = "";
        public String Description { get; set; } = "";

        public String CreditDebitType { get; set; } = "";
        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;
            


    }


}
