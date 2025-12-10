using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class StateDTO
    {
        public int StateId { get; set; }
        public string Name { get; set; } = "";
        public string Abbreviation { get; set; } = "";
        public bool IsActive { get; set; }
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}
