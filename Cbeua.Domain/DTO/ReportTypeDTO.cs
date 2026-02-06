using System;
using System.Collections.Generic;

namespace Cbeua.Domain.DTO
{
    public class ReportTypeDTO
    {
        public int ReportTypeId { get; set; }

        public string ReportTypeName { get; set; } = "";

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedDateString => CreatedDate.HasValue ? CreatedDate.Value.ToString("dd MMMM yyyy hh:mm tt") : "";

        public DateTime? ModifiedDate { get; set; }
        public string ModifiedDateString => ModifiedDate.HasValue ? ModifiedDate.Value.ToString("dd MMMM yyyy hh:mm tt") : "";

        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}