using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public partial class Accounts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AccountId { get; set; }
        public int CircleId { get; set; }
        public int BranchId { get; set; }
        public int MemeberId { get; set; }
        public int MonthCode { get; set; }
        public int YearOf { get; set; }
        public decimal Amount { get; set; }
        public int TransMode { get; set; }    
        public string Reference { get; set; }
        public string Remark { get; set; }
       
    }
}


