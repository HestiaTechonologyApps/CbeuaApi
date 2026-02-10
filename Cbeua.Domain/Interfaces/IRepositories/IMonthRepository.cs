using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IMonthRepository : IGenericRepository<Month>
    {
        Task<bool> ExistsByNameAsync(string name, int? excludeMonthCode = null);
        Task<bool> ExistsByAbbreviationAsync(string abbreviation, int? excludeMonthCode = null);
        Task<List<Month>> GetAllActiveAsync();
    }
}