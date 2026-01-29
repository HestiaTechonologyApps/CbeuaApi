using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public partial class AccountsDirectEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountsDirectEntryID { get; set; }
        public int MemberId { get; set; }
        public string Name { get; set; } = "";
        public int BranchId { get; set; }
        public int MonthCode { get; set; }
        public int YearOf { get; set; }
        public string DdIba { get; set; } = "";
        public DateTime? DdIbaDate { get; set; }
        public double? Amt { get; set; }
        public string Enrl { get; set; } = "";
        public string Fine { get; set; } = "";
        public string F9 { get; set; } = "";
        public string F10 { get; set; } = "";
        public string F11 { get; set; } = "";
        public string status { get; set; } = "";
        public Boolean isApproved { get; set; }
        public String? ApprovedBy { get; set; } = "";
        public DateTime? ApprovedDate { get; set; }
    }
}