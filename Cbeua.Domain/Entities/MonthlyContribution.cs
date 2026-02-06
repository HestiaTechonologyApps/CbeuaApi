using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cbeua.Domain.Entities
{
    public class MonthlyContribution
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MonthlyContributionId { get; set; }

        public string FileName { get; set; } = "";
        public string FileLocation { get; set; } = "";
        public string FileType { get; set; } = "";
        public string FileExtension { get; set; } = "";
        public decimal FileSize { get; set; }

        public int MonthCode { get; set; }
        public int YearOf { get; set; }
        public bool IsDeleted { get; set; } = false;

        public DateTime? CreatedDate { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int ModifiedByUserId { get; set; }
    }
}