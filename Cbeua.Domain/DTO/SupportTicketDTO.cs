using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class SupportTicketDTO
    {
        public int SupportTicketId { get; set; }

        public String SupportTicketNum { get; set; } = "";

        public String Description { get; set; } = "";

        public String Priority { get; set; } = "";

        public String Duration { get; set; } = "";

        public String DeveloperRemark { get; set; } = "";

        public Boolean isApproved { get; set; }

        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedDateSting => ApprovedDate.HasValue ? ApprovedDate.Value.ToString("dd MMMM yyyy hh:mm tt") : "";
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>(); 

    }
}
