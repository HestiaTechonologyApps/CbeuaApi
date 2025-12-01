using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.DTO
{
   
    public class AppUserListDTO
    {
        public int AppUserId { get; set; }
        public string Name { get; set; } = "";
        public string? MobileNumber { get; set; }
        public String RegisteredDate { get; set; }
        public String Status { get; set; }
        public bool IsBlocked { get; set; }
        public Decimal? WalletBalance { get; set; }

    }

    public class AppUserDTO
    {
        public int AppUserId { get; set; } 
        public int StaffUserId { get; set; } 
        public string Name { get; set; } 
        public string? Email { get; set; } 
        public string? MobileNumber { get; set; } 
        public string? Gender { get; set; } 
        public String RegisteredDate { get; set; } 
        public bool IsBlocked { get; set; } 
        public string? District { get; set; } 
        public bool IsKYCCompleted { get; set; } 
        public string? ReferralCode { get; set; } 
        public string? ReferredBy { get; set; } 
        public DateTime? LastLogin { get; set; }
        public bool IsAudultVerificationCompleted { get; set; }
        public string? Prefferedlanguage { get; set; } = "";
        public string? Interests { get; set; } = "";
        public decimal WalletBalance { get; set; } = 0m;
        public bool IsStaff { get; set; } = false;
        public String Status { get; set; }
        public string ProfileImagePath { get; set; } = ""; // Path to profile image

    }



    public class UpdateGenderDto
    {
        public int AppUserId { get; set; }
        public string Gender { get; set; }
    }

    public class UpdatePreferredLanguagesDto
    {
        public int AppUserId { get; set; }
        public List<string> PreferredLanguages { get; set; }
    }
    public class UpdateIntresetDto
    {
        public int AppUserId { get; set; }
        public List<string> Intrest { get; set; }
    }

    public class UpdateUsernameDto
    {
        public int AppUserId { get; set; }
        public string Name { get; set; }
    }
    public class UpdateStaffDto
    {
        public int AppUserId { get; set; }
        public bool IsStaff { get; set; }
    }


    public class RechargeDto
    {
        public string AppUserId { get; set; }
        public String PurchaseCouponId { get; set; }
    }
    public class RechargeResponseDto
    {
        public string AppUserId { get; set; }
        public string PurchaseOrderid { get; set; }
        public decimal Amount { get; set; }
        public Boolean IsSuccess { get; set; }
        public String ResponseText { get; set; }
        public string PaymentId { get; set; }
    }
}
