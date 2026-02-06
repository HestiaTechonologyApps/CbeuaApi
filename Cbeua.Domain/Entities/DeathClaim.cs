using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public class DeathClaim
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeathClaimId { get; set; }
        public int MemberId { get; set; }

        public int? StateId { get; set; }

        public int? DesignationId { get; set; }
        public DateTime? DeathDate { get; set; }

        public string Nominee { get; set; } = "";

        public string NomineeRelation { get; set; } = "";

        public string NomineeIDentity { get; set; } = "";


        public string DDNO { get; set; } = "";

        public DateTime? DDDATE { get; set; }   


        public decimal Amount { get; set; }

        public float LastContribution { get; set; }


        public int YearOF { get; set; }
        public bool IsDeleted { get; set; } = false;



    }
}


