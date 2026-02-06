using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IReportService
    {
        Task<List<ReportDTO>> GetAllAsync();
        Task<ReportDTO?> GetByIdAsync(int id);
        Task<ReportDTO> CreateAsync(Report report);
        Task<bool> UpdateAsync(Report report);
        Task<bool> DeleteAsync(int id);
    }
}