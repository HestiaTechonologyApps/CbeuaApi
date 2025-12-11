using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class DesignationDTO
    {
        public int DesignationId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}
