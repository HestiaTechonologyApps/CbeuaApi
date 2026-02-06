using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IReportTypeService
    {
        Task<List<ReportTypeDTO>> GetAllAsync();
        Task<ReportTypeDTO?> GetByIdAsync(int id);
        Task<ReportTypeDTO> CreateAsync(ReportType reportType);
        Task<bool> UpdateAsync(ReportType reportType);
        Task<bool> DeleteAsync(int id);
    }
}