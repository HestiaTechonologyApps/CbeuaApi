using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class DayQuoteDTO
    {
        public int DayQuoteId { get; set; }
        public int Day { get; set; }
        public int MonthCode { get; set; }
        public String ToDayQuote { get; set; } = "";
        public String UnformatedContent { get; set; } = "";
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}
