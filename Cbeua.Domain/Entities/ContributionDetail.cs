using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public class ContributionDetail
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ContributionDetailId { get; set; }
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


