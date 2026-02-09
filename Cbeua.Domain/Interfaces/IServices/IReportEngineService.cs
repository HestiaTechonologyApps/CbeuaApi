using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IReportEngineService
    {
        Task<List<ReportEngineDTO>> GetAllAsync();
        Task<ReportEngineDTO?> GetByIdAsync(int id);
        Task<ReportEngineDTO> CreateAsync(ReportEngine reportEngine);
        Task<bool> UpdateAsync(ReportEngine reportEngine);
        Task<bool> DeleteAsync(int id);
    }
}