using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.DTO
{
    public class ContributionMasterDTO
    {
        public long ContributionMasterId { get; set; }
        public string FileName { get; set; } = "";
        public string FileLocation { get; set; } = "";
        public string FileType { get; set; } = "";
        public string FileExtension { get; set; } = "";
        public decimal FileSize { get; set; }
        public string Month { get; set; } = "";
        public string Year { get; set; } = "";
        public string Circle { get; set; } = "";
        public string totalamount { get; set; } = "";
        public string totalentry { get; set; } = "";
        public string NewMemberCount { get; set; } = "";
        public string ContributionStatus { get; set; } = "";
        public bool isApproved { get; set; }
        public string ApprovedBy { get; set; } = "";
        public string ApprovedDate { get; set; } = "";
        public string MonthName { get; set; } = "";
        public int? YearOf { get; set; }  
    }
}
