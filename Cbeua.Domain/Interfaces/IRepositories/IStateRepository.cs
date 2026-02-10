using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IStateRepository : IGenericRepository<State>
    {
        Task<bool> ExistsByNameAsync(string name, int? excludeStateId = null);
        Task<bool> ExistsByAbbreviationAsync(string abbreviation, int? excludeStateId = null);
        Task<List<State>> GetAllActiveAsync();
    }
}