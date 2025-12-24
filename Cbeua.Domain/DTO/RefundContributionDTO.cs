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
        public int StaffNo { get; set; }

        public int? StateId { get; set; }

        public int? DesignationId { get; set; }
        public DateTime? DeathDate { get; set; }
        public string DeathDateString => DeathDate.HasValue ? DeathDate.Value.ToString("dd MMMM yyyy hh:mm tt") : "";

        public String RefundNO { get; set; } = "";
        public String BranchNameOFTime { get; set; } = "";
        public String DPCODEOfTime { get; set; } = "";

        public String Type { get; set; } = "";

        public String Remark { get; set; } = "";
        public string DDNO { get; set; } = "";

        public DateTime? DDDATE { get; set; }

        public string DDDATEString => DDDATE.HasValue ? DDDATE.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
        public decimal Amount { get; set; }

        public float LastContribution { get; set; }


        public int YearOF { get; set; }

        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();

    }
}
