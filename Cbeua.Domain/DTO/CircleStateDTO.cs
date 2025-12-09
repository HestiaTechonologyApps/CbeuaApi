using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class CircleStateDTO
    {
        public int CircleId { get; set; }
        public int StateId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateString => CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
        public int ModifiedByUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedDateString => ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss");
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}
