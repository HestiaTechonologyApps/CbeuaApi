using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMSJobTypeRepository : GenericRepository<HRMSJobType>, IHRMSJobTypeRepository
    {
        private readonly AppDbContext _context;

        public HRMSJobTypeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMSJobTypeDTO>> GetQuerableList()
        {
            var q = (from jobType in _context.HRMSJobTypes
                     select new HRMSJobTypeDTO
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