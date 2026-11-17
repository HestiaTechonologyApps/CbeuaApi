using System.Collections.Generic;

namespace Cbeua.Domain.DTO
{
    /// <summary>Park several specific detail rows of one contribution master in one call.</summary>
    public class BulkParkDto
    {
        public List<long> DetailIds { get; set; } = new();
        public string ParkReason { get; set; } = string.Empty;
    }

    /// <summary>
    /// Park every still-unparked discrepancy of a master in one call.
    /// ReportType: NEWMEMBERS, WRONGBRANCH, WRONGCIRCLE, or ALL (all three).
    /// </summary>
    public class ParkDiscrepanciesDto
    {
        public string ReportType { get; set; } = "ALL";
        public string ParkReason { get; set; } = string.Empty;
    }
}
