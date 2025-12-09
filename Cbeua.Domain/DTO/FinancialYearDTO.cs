using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public  class FinancialYearDTO
    {
        public int FinancialYearId { get; set; }

        public String FinacialYearCode { get; set; } = "";

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; } = false;
        public bool IsClosed { get; set; } = false;
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}
