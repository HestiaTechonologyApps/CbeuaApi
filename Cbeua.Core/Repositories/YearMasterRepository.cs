using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class YearMasterRepository : GenericRepository<YearMaster>, IYearMasterRepository
    {
        private readonly AppDbContext _context;

        public YearMasterRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // ✅ CHECK IF YEAR NAME EXISTS (excluding deleted)
        public async Task<bool> ExistsByYearNameAsync(int yearName, int? excludeYearOf = null)
        {
            var query = _context.YearMasters
                .Where(y => !y.IsDeleted && y.YearName == yearName);

            if (excludeYearOf.HasValue)
            {
                query = query.Where(y => y.YearOf != excludeYearOf.Value);
            }

            return await query.AnyAsync();
        }

        // ✅ GET ALL NON-DELETED YEARS
        public async Task<List<YearMaster>> GetAllActiveAsync()
        {
            return await _context.YearMasters
                .Where(y => !y.IsDeleted)
                .OrderBy(y => y.YearName)
                .ToListAsync();
        }
    }
}