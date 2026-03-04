using System;
using System.Collections.Generic;

namespace Cbeua.Domain.DTO.HRMS
{
    public class HRMSLeaveTypeCreateUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int MaxDaysAllowed { get; set; }
        public bool IsPaid { get; set; } = true;
        public bool CarryForward { get; set; } = false;
        public int CarryForwardLimit { get; set; } = 0;
        public string ApplicableGender { get; set; } = "All";
        public bool RequiresDocument { get; set; } = false;
        public int NoticeDaysRequired { get; set; } = 0;
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }

    public class HRMSLeaveTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int MaxDaysAllowed { get; set; }
        public bool IsPaid { get; set; }
        public bool CarryForward { get; set; }
        public int CarryForwardLimit { get; set; }
        public string ApplicableGender { get; set; } = "";
        public bool RequiresDocument { get; set; }
        public int NoticeDaysRequired { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }

    public class HRMSLeaveTypePaginationParams : BasePaginationParams
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? ApplicableGender { get; set; }
        public bool? IsPaid { get; set; }
        public bool ShowDeleted { get; set; } = false;
        public bool ShowInactive { get; set; } = true;
    }
}