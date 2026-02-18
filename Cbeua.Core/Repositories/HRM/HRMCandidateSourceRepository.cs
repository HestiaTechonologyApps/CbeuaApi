using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMCandidateSourceRepository : GenericRepository<HRMSCandidateSource>, IHRMCandidateSourceRepository
    {
        private readonly AppDbContext _context;

        public HRMCandidateSourceRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMCandidateSourceDTO>> GetQuerableList()
        {
            var q = (from source in _context.HRMSCandidateSources
                     select new HRMCandidateSourceDTO
                     {
                         Id = source.Id,
                         Name = source.Name,
                         Description = source.Description,
                         IsActive = source.IsActive,
                         IsDeleted = source.IsDeleted,
                         CreatedAt = source.CreatedAt,
                         UpdatedAt = source.UpdatedAt
                     }).AsQueryable();

            return Task.FromResult(q);
        }
    }
}