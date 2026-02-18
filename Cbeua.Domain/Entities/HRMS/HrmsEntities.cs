using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Entities.HRMS
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = false;
    }
    [Table("HRMS_Branch")]
    public class HRMBranch : BaseEntity
    {
        public String Name { get; set; }
        public String Address { get; set; }
        public String State { get; set; }
        public String Country { get; set; }
        public String ZiPcode { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
    }

    [Table("HRMS_Department")]
    public class HRMDepartment : BaseEntity
    {

        public int HRMBranchId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
       


    }

    [Table("HRMS_Designation")]
    public class HRMDesignation : BaseEntity
    {
        public int HRMDepartmentId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
    }

    [Table("HRMS_DocumentType")]
    public class HRMDocumentType : BaseEntity
    {
        public String Name { get; set; }
        public String Description { get; set; }
    }

    [Table("HRMS_AwardType")]
    public class HRMAwardType : BaseEntity
    {
        public String Name { get; set; }
        public String Description { get; set; }
    }

    [Table("HRMS_Employee")]
    public class HRMEmployee : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string FullName { get; set; }

        [MaxLength(50)]
        public string EmployeeId { get; set; } // Auto-generated (EMP000011)

        [Required]
        [MaxLength(50)]
        public string EmployeeCode { get; set; } // Biometric mapping ID

        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(20)]
        public string Gender { get; set; } // Male / Female / Other

        [Required]
        public string ProfileImagePath { get; set; }


        // ===== Employment Details =====

        [Required]
        public int HRMBranchId { get; set; }

        [Required]
        public int HRMDepartmentId { get; set; }

        [Required]
        public int HRMDesignationId { get; set; }

        [Required]
        public DateTime DateOfJoining { get; set; }

        [Required]
        [MaxLength(50)]
        public string EmploymentType { get; set; } // Full-time / Part-time / etc.

        [Required]
        [MaxLength(50)]
        public string EmployeeStatus { get; set; } // Active / Inactive / etc.

        public int? ShiftId { get; set; }

        public int? AttendancePolicyId { get; set; }


        // ===== Contact Information =====

        [Required]
        [MaxLength(200)]
        public string AddressLine1 { get; set; }

        [Required]
        [MaxLength(200)]
        public string AddressLine2 { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string State { get; set; }

        [Required]
        [MaxLength(100)]
        public string Country { get; set; }

        [MaxLength(20)]
        public string PostalCode { get; set; }


        // ===== Emergency Contact =====

        [Required]
        [MaxLength(150)]
        public string EmergencyContactName { get; set; }

        [Required]
        [MaxLength(100)]
        public string EmergencyContactRelationship { get; set; }

    }

    [Table("HRMS_EmployeeAward")]
    public class HrmEmployeeAward : BaseEntity
    {
        public int HRMEmployeeId { get; set; }
        public int HrmAwardTypeId { get; set; }

        public DateTime AwardDate { get; set; }
        public string Gift { get; set; }
        public string Description { get; set; }
        public Decimal MonetaryValue { get; set; }

        
    }
    [Table("HRMS_JobCategories")]
    public class HRMSJobCategory : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
       


    }
    [Table("HRMS_JobTypes")]
    public class HRMSJobType : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }



    }
    [Table("HRMS_JobLocations")]
    public class HRMSJobLocation : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsRemote { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string State { get; set; }

        [Required]
        [MaxLength(100)]
        public string Country { get; set; }

        [MaxLength(20)]
        public string PostalCode { get; set; }


    }

    [Table("HRMS_CandidateSources")]
    public class HRMSCandidateSource : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }



    }


    [Table("HRMS_InterviewTypes")]
    public class HRMSInterviewType : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }



    }

    [Table("HRMS_Jobs")]
    public class HRMSJob : BaseEntity 
    {
       

        // Basic Information
        public string JobTitle { get; set; }
       
        public string Location { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string ExistingLink { get; set; }
        public int NumberOfOpenings { get; set; }

        // Experience & Salary
        public int MinimumExperienceYears { get; set; }
        public int MaximumExperienceYears { get; set; }

        public decimal? MinimumSalary { get; set; }
        public decimal? MaximumSalary { get; set; }

        // Job Details
        public string JobDescription { get; set; }
        public string JobRequrement { get; set; }
        public string JobBenefits { get; set; }

       

        // Audit
       
    }

    [Table("HRMS_Candidate")]
    public class HRMSCandidate : BaseEntity
    {
        

        // Contact Information
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        // Professional Information
        public int ExperienceInYears { get; set; }
        public decimal CurrentSalary { get; set; }
        public decimal ExpectedSalary { get; set; }
        public string NoticePeriod { get; set; }
        // Audit Fields
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }


  
       
}
