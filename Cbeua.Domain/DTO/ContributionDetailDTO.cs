
namespace Cbeua.Domain.DTO
{
    public class ContributionDetailDTO
    {
        public string FullString { get; set; } = "";
        public int Circle { get; set; }
        public string Month { get; set; } = "";
        public string Year { get; set; } = "";
        public string DpCode { get; set; } = "";
        public string StaffNo { get; set; } = "";
        public string Name { get; set; } = "";
        public string Designation { get; set; } = "";
        public int Amount { get; set; }
    }
  
        public class ContributionDetailPaginationParams
        {
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 50;
            public bool GetAll { get; set; } = false;
            public string? StaffNo { get; set; }
            public string? Name { get; set; }
            public string? DpCode { get; set; }
            public bool? IsParked { get; set; }
            public string? SearchTerm { get; set; }
            public string? SortBy { get; set; }
            public bool SortDescending { get; set; } = false;
        }
    public class DefaulterDTO
    {
        public int MemberId { get; set; }
        public int StaffNo { get; set; }
        public string Name { get; set; } = "";
        public int? BranchId { get; set; }
    }
     public class ParkedDetailDto
        {
            public long ContributionDetailId { get; set; }
            public long ContributionMasterId { get; set; }
            public string FullString { get; set; } = "";
            public int Circle { get; set; }
            public string Month { get; set; } = "";
            public string Year { get; set; } = "";
            public string DpCode { get; set; } = "";
            public string StaffNo { get; set; } = "";
            public string Name { get; set; } = "";
            public string Designation { get; set; } = "";
            public decimal Amount { get; set; } 
            public string Total { get; set; } = "";
            public string ParkReason { get; set; } = "";
    }
 
    public class ContributionMasterListDTO
    {
        public long ContributionMasterId { get; set; }
        public string? FileName { get; set; }
        public string? FileLocation { get; set; }
        public string? FileType { get; set; }
        public string? FileExtension { get; set; }
        public decimal FileSize { get; set; }
        public string? Month { get; set; }
        public string? Year { get; set; }
        public string? Circle { get; set; }
        public string? TotalAmount { get; set; }
        public string? TotalEntry { get; set; }
        public string? ContributionStatus { get; set; }
        public string? NewMemberCount { get; set; }
        public string? ApprovedBy { get; set; }
        public string? ApprovedDate { get; set; }
        public bool IsApproved { get; set; }
    }
    public class ContributionParseResultDTO
    {
        public int TotalEntry { get; set; }
        public int TotalAmount { get; set; }
        public int ErrorCount { get; set; }
        public List<ContributionDetailDTO> ValidLines { get; set; } = new();
        public List<string> ErrorLines { get; set; } = new();
    }
}