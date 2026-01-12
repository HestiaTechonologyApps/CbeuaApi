using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class BranchDTO
    {
        public int BranchId { get; set; }
        public int DpCode { get; set; }
        public string Name { get; set; } = "";
        public string Address1 { get; set; } = "";
        public string Address2 { get; set; } = "";
        public string Address3 { get; set; } = "";
        public string District { get; set; } = "";
        public string Status { get; set; } = "";
        public int CircleId { get; set; }
        public string CircleName { get; set; } = "";
        public int? StateId { get; set; }
        public string StateName { get; set; } = "";
        public bool IsRegCompleted { get; set; }
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}