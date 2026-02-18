using System;
using System.Collections.Generic;

namespace Cbeua.Domain.DTO.HRMS
{
    public class HRMEmployeeCreateUpdateDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string EmployeeCode { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = "";
        public string ProfileImagePath { get; set; } = "";

        // Employment
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string EmploymentType { get; set; } = "";
        public string EmployeeStatus { get; set; } = "";
        public int? ShiftId { get; set; }
        public int? AttendancePolicyId { get; set; }

        // Contact
        public string AddressLine1 { get; set; } = "";
        public string AddressLine2 { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Country { get; set; } = "";
        public string PostalCode { get; set; } = "";

        // Emergency
        public string EmergencyContactName { get; set; } = "";
        public string EmergencyContactRelationship { get; set; } = "";

        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }

    public class HRMEmployeeDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string EmployeeId { get; set; } = "";
        public string EmployeeCode { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = "";
        public string ProfileImagePath { get; set; } = "";

        // Employment
        public int BranchId { get; set; }
        public string BranchName { get; set; } = "";
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = "";
        public int DesignationId { get; set; }
        public string DesignationName { get; set; } = "";
        public DateTime DateOfJoining { get; set; }
        public string EmploymentType { get; set; } = "";
        public string EmployeeStatus { get; set; } = "";
        public int? ShiftId { get; set; }
        public int? AttendancePolicyId { get; set; }

        // Contact
        public string AddressLine1 { get; set; } = "";
        public string AddressLine2 { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Country { get; set; } = "";
        public string PostalCode { get; set; } = "";

        // Emergency
        public string EmergencyContactName { get; set; } = "";
        public string EmergencyContactRelationship { get; set; } = "";

        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }

    public class HRMEmployeePaginationParams : BasePaginationParams
    {
        public int? Id { get; set; }
        public int? BranchId { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? EmploymentType { get; set; }
        public string? EmployeeStatus { get; set; }
        public bool ShowDeleted { get; set; } = false;
        public bool ShowInactive { get; set; } = true;
    }
}