using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = "";
        public string Abbreviation { get; set; } = "";
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}
