using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public class RefundContribution
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RefundContributionId { get; set; }
     

        public int? StateId { get; set; }
        public int MemberId { get; set; }

        public int? DesignationId { get; set; }
       

        public String RefundNO { get; set; }
        public String BranchNameOFTime { get; set; }
        public String DPCODEOfTime { get; set; }

        public String Type { get; set; }

        public String Remark { get; set; }
        public string DDNO { get; set; }

        public DateTime? DDDATE { get; set; }


        public decimal Amount { get; set; }

        public float LastContribution { get; set; }


        public int YearOF { get; set; }

      

      
    }
}


