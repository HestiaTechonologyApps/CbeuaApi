using System;
using System.Collections.Generic;

namespace Cbeua.Domain.DTO.HRMS
{
    public class HRMEmployeeAwardCreateUpdateDTO
    {
        public int Id { get; set; }
        public int HRMEmployeeId { get; set; }
        public int HrmAwardTypeId { get; set; }
        public DateTime AwardDate { get; set; }
        public string Gift { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal MonetaryValue { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }

    public class HRMEmployeeAwardDTO
    {
        public int Id { get; set; }
        public int HRMEmployeeId { get; set; }
        public string EmployeeName { get; set; } = "";
        public string EmployeeCode { get; set; } = "";
        public int HrmAwardTypeId { get; set; }
        public string AwardTypeName { get; set; } = "";
        public DateTime AwardDate { get; set; }
        public string Gift { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal MonetaryValue { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }

    public class HRMEmployeeAwardPaginationParams : BasePaginationParams
    {
        public int? Id { get; set; }
        public int? HRMEmployeeId { get; set; }
        public int? HrmAwardTypeId { get; set; }
        public DateTime? AwardDateFrom { get; set; }
        public DateTime? AwardDateTo { get; set; }
        public string? Gift { get; set; }
        public bool ShowDeleted { get; set; } = false;
        public bool ShowInactive { get; set; } = true;
    }
}