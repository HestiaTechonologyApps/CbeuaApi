using Cbeua.Domain.DTO;
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
    public class MonthRepository : GenericRepository<Month>, IMonthRepository
    {
        private readonly AppDbContext _context;

        public MonthRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<MonthDTO> GetQueryableMonths()
        {
            var q = from m in _context.Months
                    where !m.IsDeleted // ✅ FILTER OUT DELETED
                    select new MonthDTO
                    {
                        MonthCode = m.MonthCode,
                        MonthName = m.MonthName,
                        Abbrivation = m.Abbrivation,
                        IsDeleted = m.IsDeleted
                    };
            return q;
        }

        // ✅ CHECK IF MONTH NAME EXISTS (case-insensitive, trimmed, excluding deleted)
        public async Task<bool> ExistsByNameAsync(string name, int? excludeMonthCode = null)
        {
            var normalizedName = name.Trim().ToLower();

            var query = _context.Months
                .Where(m => !m.IsDeleted && m.MonthName.Trim().ToLower() == normalizedName);

            if (excludeMonthCode.HasValue)
            {
                query = query.Where(m => m.MonthCode != excludeMonthCode.Value);
            }

            return await query.AnyAsync();
        }

        // ✅ CHECK IF ABBREVIATION EXISTS (case-insensitive, trimmed, excluding deleted)
        public async Task<bool> ExistsByAbbreviationAsync(string abbreviation, int? excludeMonthCode = null)
        {
            var normalizedAbbr = abbreviation.Trim().ToLower();

            var query = _context.Months
                .Where(m => !m.IsDeleted && m.Abbrivation.Trim().ToLower() == normalizedAbbr);

            if (excludeMonthCode.HasValue)
            {
                query = query.Where(m => m.MonthCode != excludeMonthCode.Value);
            }

            return await query.AnyAsync();
        }

        // ✅ GET ALL NON-DELETED MONTHS
        public async Task<List<Month>> GetAllActiveAsync()
        {
            return await _context.Months
                .Where(m => !m.IsDeleted)
                .ToListAsync();
        }
    }
}