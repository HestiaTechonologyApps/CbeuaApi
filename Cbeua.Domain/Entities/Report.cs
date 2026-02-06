using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public partial class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportId { get; set; }

        public int ReportTypeId { get; set; }

        public int YearOf { get; set; }

        public int MonthCode { get; set; }

        public int CircleId { get; set; }

        public int BranchId { get; set; }

        public int MemberId { get; set; }
        public bool IsDeleted { get; set; } = false;

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; }
    }
}