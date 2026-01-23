using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Cbeua.Domain.DTO
{
    public class MonthlyContributionDTO
    {
        public long MonthlyContributionId { get; set; }
        public string FileName { get; set; } = "";
        public string FileLocation { get; set; } = "";
        public string FileType { get; set; } = "";
        public string FileExtension { get; set; } = "";
        public decimal FileSize { get; set; }

        public int MonthCode { get; set; }
        public string MonthName { get; set; } = "";

        public int YearOf { get; set; }
        public int YearName { get; set; }

        public List<AuditLogDTO> AuditLogs { get; set; } = new List<AuditLogDTO>();
    }

    // Upload DTO
    public class MonthlyContributionFileUploadDto
    {
        public int MonthCode { get; set; }
        public int YearOf { get; set; }
        public IFormFile ContributionFile { get; set; }
    }
}