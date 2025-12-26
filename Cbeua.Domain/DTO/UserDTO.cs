using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Domain.DTO;

namespace Cbeua.Domain.DTO
{
    public class UserDTO
    {

        public int UserId { get; set; } // Primary Key
        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public bool IsActive { get; set; } = true;
        public bool Islocked { get; set; } = false;
        public int CompanyId { get; set; } =0;
        public string ComapanyName { get; set; } = "";
        public DateTime? CreateAt { get; set; } = DateTime.UtcNow;
        public string CreateAtString { get; set; } = "";
        public DateTime? Lastlogin { get; set; }
        public string LastloginString { get; set; } = "";
        public string CreateAtSyring { get; set; } = "";
        public bool IsDeleted { get; set; } = false;
        public string Role { get; set; }
        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }

    public class UserListDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public bool IsActive { get; set; }
    }



    public class PasswordChangeRequest
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
    }
}
