using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class AccountsDirectEntryDTO
    {
        public int AccountsDirectEntryID { get; set; }
        public int MemberId { get; set; }
        public string Name { get; set; } = "";
        public int BranchId { get; set; }
        public string MemberName { get; set; } = "";
        public string BranchName { get; set; } = "";
        public string MonthName { get; set; } = "";
        public int MonthCode { get; set; }
        public int YearOf { get; set; }
        public int YearName { get; set; }
        public string DdIba { get; set; } = "";
        public DateTime? DdIbaDate { get; set; }
        public string DdIbaDateString => DdIbaDate?.ToString("dd MMMM yyyy hh:mm tt") ?? "";
        public double? Amt { get; set; }
        public string Enrl { get; set; } = "";
        public string Fine { get; set; } = "";
        public string F9 { get; set; } = "";
        public string F10 { get; set; } = "";
        public string F11 { get; set; } = "";
        public string status { get; set; } = "";
        public Boolean isApproved { get; set; }
        public String ApprovedBy { get; set; } = "";
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedDateString => ApprovedDate?.ToString("dd MMMM yyyy hh:mm tt") ?? "";
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}