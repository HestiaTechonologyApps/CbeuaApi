using System;

namespace Cbeua.Domain.DTO
{
    public class UserRegistrationDTO
    {
        public int UserRegistrationId { get; set; }
        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public int StaffNo { get; set; }
        public int? MemberId { get; set; }
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public string Role { get; set; } = "";
        public string RegistrationStatus { get; set; } = "";
        public DateTime RequestedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? RejectReason { get; set; }
    }

    public class UserRegistrationListDTO
    {
        public int UserRegistrationId { get; set; }
        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public int StaffNo { get; set; }
        public string PhoneNumber { get; set; } = "";
        public string RegistrationStatus { get; set; } = "";
        public DateTime RequestedDate { get; set; }
    }
}