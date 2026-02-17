using System;
using System.Collections.Generic;

namespace Cbeua.Domain.DTO.HRMS
{
    public class HRMDepartmentCreateUpdateDTO
    {
        public int Id { get; set; }
        public int HRMBranchId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }

    public class HRMDepartmentDTO
    {
        public int Id { get; set; }
        public int HRMBranchId { get; set; }
        public string BranchName { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }

    public class HRMDepartmentPaginationParams : BasePaginationParams
    {
        public int? Id { get; set; }
        public int? HRMBranchId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool ShowDeleted { get; set; } = false;
        public bool ShowInactive { get; set; } = true;
    }
}