using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMInterviewTypeRepository : GenericRepository<HRMSInterviewType>, IHRMInterviewTypeRepository
    {
        private readonly AppDbContext _context;
        public HRMInterviewTypeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMInterviewTypeDTO>> GetQuerableList()
        {
            var q = (from interviewType in _context.HRMSInterviewTypes
                     select new HRMInterviewTypeDTO
                     {
                         Id = interviewType.Id,
                         Name = interviewType.Name,
                         Description = interviewType.Description,
                         IsActive = interviewType.IsActive,
                         IsDeleted = interviewType.IsDeleted,
                         CreatedAt = interviewType.CreatedAt,
                         UpdatedAt = interviewType.UpdatedAt
                     }).AsQueryable();
            return Task.FromResult(q);
        }
    }
}