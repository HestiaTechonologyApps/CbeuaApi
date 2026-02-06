using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class DeathClaimDTO
    {
        public int DeathClaimId { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public int StaffNo { get; set; }
        public int? StateId { get; set; }

        public string StateName { get; set; }

        public int? DesignationId { get; set; }

        public string DesignationName { get; set; }
        public DateTime? DeathDate { get; set; }
        public static string DeathDatestring => DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") ?? "";
        public string Nominee { get; set; } = "";

        public string NomineeRelation { get; set; } = "";

        public string NomineeIDentity { get; set; } = "";


        public string DDNO { get; set; } = "";

        public DateTime? DDDATE { get; set; }
        public static string DDDATEString => DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") ?? "";

        public decimal Amount { get; set; }

        public float LastContribution { get; set; }


        public int YearOF { get; set; }
        public int YearName { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
