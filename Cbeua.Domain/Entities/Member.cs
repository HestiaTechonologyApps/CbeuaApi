using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Cbeua.Domain.Entities
{
    public class Member
    {





        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int MemberId { get; set; }

         public int StaffNo { get; set; }




        public int? DesignationId { get; set; }
        public int? CategoryId { get; set; }
        public int? BranchId { get; set; }



        public string Name { get; set; }
        public int GenderId { get; set; }
        public int? ImageId { get; set; }
        public DateTime? Dob { get; set; }

        public DateTime? Doj { get; set; }

        public DateTime? DojtoScheme { get; set; }
        public int StatusId { get; set; }
        public bool IsRegCompleted { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedByUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string Nominee { get; set; }
        public string ProfileImageSrc { get; set; }

        public string NomineeRelation { get; set; }

        public string NomineeIDentity { get; set; }


        public string UnionMember { get; set; }

        [NotMapped]
        public string Gender_text { get; set; }
        [NotMapped]
        public string Designation_text { get; set; }
        [NotMapped]
        public string Category_text { get; set; }
        [NotMapped]
        public string Status_text { get; set; }
        [NotMapped]
        public string TotalContribution { get; set; }

        [NotMapped]
        public string TotalRefund { get; set; }
    }

    public class ContributionDetail
    {
        public ContributionDetail()
        {
            isParked = false;
        }
        public long Id { get; set; }
        public String FullString { get; set; }
        public int Circle { get; set; }
        public String Month { get; set; }
        public String Year { get; set; }
        public String DpCode { get; set; }
        public String StaffNo { get; set; }
        public String Name { get; set; }
        public String Designation { get; set; }

        public Decimal Amount { get; set; }
        public Boolean isParked { get; set; }

        public long ContributionMasterId { get; set; }

        public String ParkReason { get; set; } = "";

        public DateTime? Parkedon { get; set; }

        public DateTime? UnParkedon { get; set; }

        public String Total { get; set; }
    }
}


