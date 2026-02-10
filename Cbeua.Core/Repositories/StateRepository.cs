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
    public class StateRepository : GenericRepository<State>, IStateRepository
    {
        private readonly AppDbContext _context;

        public StateRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // ✅ CHECK IF STATE NAME EXISTS (case-insensitive, trimmed, excluding deleted)
        public async Task<bool> ExistsByNameAsync(string name, int? excludeStateId = null)
        {
            var normalizedName = name.Trim().ToLower();

            var query = _context.States
                .Where(s => !s.IsDeleted && s.Name.Trim().ToLower() == normalizedName);

            if (excludeStateId.HasValue)
            {
                query = query.Where(s => s.StateId != excludeStateId.Value);
            }

            return await query.AnyAsync();
        }

        // ✅ CHECK IF ABBREVIATION EXISTS (case-insensitive, trimmed, excluding deleted)
        public async Task<bool> ExistsByAbbreviationAsync(string abbreviation, int? excludeStateId = null)
        {
            var normalizedAbbr = abbreviation.Trim().ToLower();

            var query = _context.States
                .Where(s => !s.IsDeleted && s.Abbreviation.Trim().ToLower() == normalizedAbbr);

            if (excludeStateId.HasValue)
            {
                query = query.Where(s => s.StateId != excludeStateId.Value);
            }

            return await query.AnyAsync();
        }

        // ✅ GET ALL NON-DELETED STATES
        public async Task<List<State>> GetAllActiveAsync()
        {
            return await _context.States
                .Where(s => !s.IsDeleted)
                .ToListAsync();
        }
    }
}