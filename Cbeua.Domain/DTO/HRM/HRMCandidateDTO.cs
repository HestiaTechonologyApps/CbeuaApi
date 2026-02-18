using System;
using System.Collections.Generic;

namespace Cbeua.Domain.DTO.HRMS
{
    public class HRMCandidateCreateUpdateDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Country { get; set; } = "";
        public string ZipCode { get; set; } = "";
        public int ExperienceInYears { get; set; } = 0;
        public decimal CurrentSalary { get; set; } = 0;
        public decimal ExpectedSalary { get; set; } = 0;
        public string NoticePeriod { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }

    public class HRMCandidateDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Country { get; set; } = "";
        public string ZipCode { get; set; } = "";
        public int ExperienceInYears { get; set; }
        public decimal CurrentSalary { get; set; }
        public decimal ExpectedSalary { get; set; }
        public string NoticePeriod { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }

    public class HRMCandidatePaginationParams : BasePaginationParams
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? NoticePeriod { get; set; }
        public bool ShowDeleted { get; set; } = false;
        public bool ShowInactive { get; set; } = true;
    }
}