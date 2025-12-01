using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Cbeua.Domain.Entities.Common
{
    public class AuditLog
    {
        [Key]
        public Guid LogID { get; set; }
        public string TableName { get; set; }
        public string Action { get; set; }
        public int? RecordID { get; set; }

        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangeDetails { get; set; }

    }
}
