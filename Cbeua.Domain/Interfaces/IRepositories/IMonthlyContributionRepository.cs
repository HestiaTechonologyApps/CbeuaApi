using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IMonthlyContributionRepository : IGenericRepository<MonthlyContribution>
    {
        //IQueryable<MonthlyContributionDTO> GetQueryableMonthlyContributionList();
        //Task<IEnumerable<MonthlyContributionDTO>> GetAllAsync();
        //Task<MonthlyContributionDTO> GetByIdDTOAsync(long id);
    }
}