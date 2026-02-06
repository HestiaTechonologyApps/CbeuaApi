using System;
using System.Collections.Generic;

namespace Cbeua.Domain.DTO
{
    public class ReportDTO
    {
        public int ReportId { get; set; }

        public int ReportTypeId { get; set; }
        public string ReportTypeName { get; set; } = "";

        public int YearOf { get; set; }
        public string YearName { get; set; } = "";

        public int MonthCode { get; set; }
        public string MonthName { get; set; } = "";

        public int CircleId { get; set; }
        public string CircleName { get; set; } = "";

        public int BranchId { get; set; }
        public int? DpCode { get; set; }
        public string BranchName { get; set; } = "";

        public int MemberId { get; set; }
        public string MemberName { get; set; } = "";
        public int? StaffNo { get; set; }
        public bool IsDeleted { get; set; } = false;

        public DateTime? CreatedDate { get; set; }
        public string CreatedDateString => CreatedDate.HasValue ? CreatedDate.Value.ToString("dd MMMM yyyy hh:mm tt") : "";

        public DateTime? ModifiedDate { get; set; }
        public string ModifiedDateString => ModifiedDate.HasValue ? ModifiedDate.Value.ToString("dd MMMM yyyy hh:mm tt") : "";

        public bool IsActive { get; set; }

        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}