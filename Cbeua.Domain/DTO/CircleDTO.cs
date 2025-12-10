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
        public DateTime? DateFrom { get; set; }
        public string DateFromString => DateFrom.HasValue ? DateFrom.Value.ToString("yyyy-MM-dd") : "";
        public DateTime? DateTo { get; set; }
        public string DateToString => DateTo.HasValue ? DateTo.Value.ToString("yyyy-MM-dd") : "";
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}
