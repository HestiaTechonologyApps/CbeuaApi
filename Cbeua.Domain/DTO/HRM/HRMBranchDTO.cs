using System;
using System.Collections.Generic;

namespace Cbeua.Domain.DTO.HRMS
{
    public class HRMBranchCreateUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string State { get; set; } = "";
        public string Country { get; set; } = "";
        public string ZiPcode { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }

    public class HRMBranchDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string State { get; set; } = "";
        public string Country { get; set; } = "";
        public string ZiPcode { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }

    public class HRMBranchPaginationParams : BasePaginationParams
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public bool ShowDeleted { get; set; } = false;
        public bool ShowInactive { get; set; } = true;
    }
}