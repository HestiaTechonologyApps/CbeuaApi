
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

    public class ContributionParseResultDTO
    {
        public int TotalEntry { get; set; }
        public int TotalAmount { get; set; }
        public int ErrorCount { get; set; }
        public List<ContributionDetailDTO> ValidLines { get; set; } = new();
        public List<string> ErrorLines { get; set; } = new();
    }
}