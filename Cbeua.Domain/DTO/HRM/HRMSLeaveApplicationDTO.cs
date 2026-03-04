using System;
using System.Collections.Generic;

namespace Cbeua.Domain.DTO.HRMS
{
    public class HRMSLeaveApplicationCreateUpdateDTO
    {
        public int Id { get; set; }
        public int HRMEmployeeId { get; set; }
        public int HRMSLeaveTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalDays { get; set; }
        public string DayType { get; set; } = "Full Day";
        public string Reason { get; set; } = "";
        public string Status { get; set; } = "Pending";
        public string DocumentUrl { get; set; } = "";
        public int? ReviewedBy { get; set; }
        public DateTime? ReviewedOn { get; set; }
        public string ReviewerRemarks { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }

    public class HRMSLeaveApplicationDTO
    {
        public int Id { get; set; }
        public int HRMEmployeeId { get; set; }
        public string EmployeeName { get; set; } = "";
        public int HRMSLeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; } = "";
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalDays { get; set; }
        public string DayType { get; set; } = "";
        public string Reason { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime AppliedOn { get; set; }
        public string DocumentUrl { get; set; } = "";
        public int? ReviewedBy { get; set; }
        public string ReviewedByName { get; set; } = "";
        public DateTime? ReviewedOn { get; set; }
        public string ReviewerRemarks { get; set; } = "";
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }

    public class HRMSLeaveApplicationPaginationParams : BasePaginationParams
    {
        public int? Id { get; set; }
        public int? HRMEmployeeId { get; set; }
        public int? HRMSLeaveTypeId { get; set; }
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool ShowDeleted { get; set; } = false;
        public bool ShowInactive { get; set; } = true;
    }
}