using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class UserLoginLogDTO
    {
        public int UserLoginLogId { get; set; }
        public int UserId { get; set; }
        public DateTime ActionTime { get; set; }
        public string ActionTimeString { get; set; } = "";
        public string ActionType { get; set; } = "Login";
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();

    }
}
