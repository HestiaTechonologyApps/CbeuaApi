using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class MemberDTO
    {
        public int MemberId { get; set; }

        public int StaffNo { get; set; }

        public string DesignationName { get; set; } = "";
        public string BranchName { get; set; } = "";
        public string Categoryname { get; set; } = "";
        public string Status { get; set; } = "";
        public string Gender { get; set; } = "";
        public string DpCode { get; set; } = "";


        public int? DesignationId { get; set; }
        public int? CategoryId { get; set; }
        public int? BranchId { get; set; }
        public string Name { get; set; } = "";
        public int GenderId { get; set; }
        public int? ImageId { get; set; }
        public DateTime? Dob { get; set; }
        public string DobString => Dob?.ToString("dd MMMM yyyy hh:mm tt") ?? "";

        public DateTime? Doj { get; set; }
        public string DojString => Doj?.ToString("dd MMMM yyyy hh:mm tt") ?? "";
        public DateTime? DojtoScheme { get; set;}
        public string DojtoSchemeString => DojtoScheme?.ToString("dd MMMM yyyy hh:mm tt") ?? "";
        public int StatusId { get; set; }
        public bool IsRegCompleted { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateString => CreatedDate?.ToString("dd MMMM yyyy hh:mm tt") ?? "";
        public int ModifiedByUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedDateString => ModifiedDate?.ToString("dd MMMM yyyy hh:mm tt") ?? "";
        public string Nominee { get; set; } = "";
        public string ProfileImageSrc { get; set; } = "";
        public string NomineeRelation { get; set; } = "";
        public string NomineeIDentity { get; set; } = "";
        public string UnionMember { get; set; } = "";
        public string TotalRefund { get; set; } = "";
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }
}
public class ProfilePicUploadDto
{
    public int MemberId { get; set; }
    public IFormFile ProfilePic { get; set; }
}

