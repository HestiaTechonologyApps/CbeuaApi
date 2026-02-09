using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class ReportEngineDTO
    {
        public int ReportEngineId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string SQLString { get; set; } = "";
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedDateString { get; set; } = "";
        public string ModifiedDateString { get; set; } = "";
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}