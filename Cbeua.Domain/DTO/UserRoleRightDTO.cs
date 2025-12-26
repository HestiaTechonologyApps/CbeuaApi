using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class UserRoleRightDTO
    {
        public int UserRoleRightId { get; set; }

        public DateTime ControllerName { get; set; }
        public string ControllerNameString => ControllerName.ToString("dd MMMM yyyy hh:mm tt");

        public String ActionName { get; set; } = "";

        public int UserTypeID { get; set; }
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();

    }
}
