using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public class UserRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRegistrationId { get; set; }
        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public int StaffNo { get; set; }
        public int? MemberId { get; set; }
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Role { get; set; } = "OfficeStaff";
        public int CompanyId { get; set; } = 1;

        public string RegistrationStatus { get; set; } = "Pending";
        public DateTime RequestedDate { get; set; } = DateTime.UtcNow;
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? RejectReason { get; set; }
    }
}