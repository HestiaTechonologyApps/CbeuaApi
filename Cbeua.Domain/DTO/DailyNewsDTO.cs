using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class DailyNewsDTO
    {
        public int DailyNewsId { get; set; }


        public string Title { get; set; }

        public string Description { get; set; } = "";

        public DateTime NewsDate { get; set; }
        public string NewsDateString => NewsDate.ToString("yyyy-MM-dd HH:mm:ss");

        public int CompanyId { get; set; } = 0;

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedOnString => CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");

        public string CreatedBy { get; set; } = "";

        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}
