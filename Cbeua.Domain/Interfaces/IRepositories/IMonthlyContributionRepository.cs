using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System.Linq;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IMonthlyContributionRepository : IGenericRepository<MonthlyContribution>
    {
        IQueryable<MonthlyContributionDTO> GetQueryableMonthlyContributions();
    }
}