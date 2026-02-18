using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMAwardTypeRepository : GenericRepository<HRMAwardType>, IHRMAwardTypeRepository
    {
        private readonly AppDbContext _context;

        public HRMAwardTypeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMAwardTypeDTO>> GetQuerableList()
        {
            var q = (from awardType in _context.HRMAwardTypes
                     select new HRMAwardTypeDTO
                     {
                         Id = awardType.Id,
                         Name = awardType.Name,
                         Description = awardType.Description,
                         IsActive = awardType.IsActive,
                         IsDeleted = awardType.IsDeleted,
                         CreatedAt = awardType.CreatedAt,
                         UpdatedAt = awardType.UpdatedAt
                     }).AsQueryable();

            return Task.FromResult(q);
        }
    }
}