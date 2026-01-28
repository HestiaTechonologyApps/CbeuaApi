using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class RefundContributionDTO
    {
        public int RefundContributionId { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; } = "";
        public int StaffNo { get; set; }
        public int? StateId { get; set; }
        public string StateName { get; set; } = "";
        public int? DesignationId { get; set; }
        public string DesignationName { get; set; } = "";
        public string RefundNO { get; set; } = "";
        public string BranchNameOFTime { get; set; } = "";
        public string DPCODEOfTime { get; set; } = "";
        public string Type { get; set; } = "";
        public string Remark { get; set; } = "";
        public string DDNO { get; set; } = "";
        public DateTime? DDDATE { get; set; }
        public decimal Amount { get; set; }
        public float LastContribution { get; set; }
        public int YearOF { get; set; }
        public int YearName { get; set; }
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}