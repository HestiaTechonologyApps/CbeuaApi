using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMJobTypeRepository : GenericRepository<HRMSJobType>, IHRMJobTypeRepository
    {
        private readonly AppDbContext _context;

        public HRMJobTypeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMJobTypeDTO>> GetQuerableList()
        {
            var q = (from jobType in _context.HRMSJobTypes
                     select new HRMJobTypeDTO
                     {
                         Id = jobType.Id,
                         Name = jobType.Name,
                         Description = jobType.Description,
                         IsActive = jobType.IsActive,
                         IsDeleted = jobType.IsDeleted,
                         CreatedAt = jobType.CreatedAt,
                         UpdatedAt = jobType.UpdatedAt
                     }).AsQueryable();

            return Task.FromResult(q);
        }
    }
}