using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class CircleDTO
    {
        public int CircleId { get; set; }

        public int? CircleCode { get; set; }
        public string Name { get; set; } = "";
        public string Abbreviation { get; set; } = "";
        public bool IsActive { get; set; }
        public int? StateId { get; set; }
        public string StateName { get; set; } = "";
        public DateTime? DateFrom { get; set; }
        public string DateFromString => DateFrom.HasValue ? DateFrom.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
        public DateTime? DateTo { get; set; }
        public string DateToString => DateTo.HasValue ? DateTo.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}
