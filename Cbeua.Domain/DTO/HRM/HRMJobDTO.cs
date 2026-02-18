using System;
using System.Collections.Generic;

namespace Cbeua.Domain.DTO.HRMS
{
    public class HRMJobCreateUpdateDTO
    {
        public int Id { get; set; }
        public string JobTitle { get; set; } = "";
        public string Location { get; set; } = "";
        public string Branch { get; set; } = "";
        public string Department { get; set; } = "";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ExistingLink { get; set; } = "";
        public int NumberOfOpenings { get; set; }
        public int MinimumExperienceYears { get; set; }
        public int MaximumExperienceYears { get; set; }
        public decimal? MinimumSalary { get; set; }
        public decimal? MaximumSalary { get; set; }
        public string JobDescription { get; set; } = "";
        public string JobRequrement { get; set; } = "";
        public string JobBenefits { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }

    public class HRMJobDTO
    {
        public int Id { get; set; }
        public string JobTitle { get; set; } = "";
        public string Location { get; set; } = "";
        public string Branch { get; set; } = "";
        public string Department { get; set; } = "";
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ExistingLink { get; set; } = "";
        public int NumberOfOpenings { get; set; }
        public int MinimumExperienceYears { get; set; }
        public int MaximumExperienceYears { get; set; }
        public decimal? MinimumSalary { get; set; }
        public decimal? MaximumSalary { get; set; }
        public string JobDescription { get; set; } = "";
        public string JobRequrement { get; set; } = "";
        public string JobBenefits { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }

    public class HRMJobPaginationParams : BasePaginationParams
    {
        public int? Id { get; set; }
        public string? JobTitle { get; set; }
        public string? Location { get; set; }
        public string? Branch { get; set; }
        public string? Department { get; set; }
        public bool ShowDeleted { get; set; } = false;
        public bool ShowInactive { get; set; } = true;
    }
}