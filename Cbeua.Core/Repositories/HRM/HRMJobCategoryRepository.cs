using Cbeua.Domain.DTO.HRM;
using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMJobCategoryRepository : GenericRepository<HRMSJobCategory>, IHRMJobCategoryRepository
    {
        private readonly AppDbContext _context;
        public HRMJobCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMJobCategoryDTO>> GetQuerableList()
        {
            var q = (from jobCategory in _context.HRMSJobCategorys
                     select new HRMJobCategoryDTO
                     {
                         Id = jobCategory.Id,
                         Name = jobCategory.Name,
                         Description = jobCategory.Description,
                         IsActive = jobCategory.IsActive,
                         IsDeleted = jobCategory.IsDeleted,
                         CreatedAt = jobCategory.CreatedAt,
                         UpdatedAt = jobCategory.UpdatedAt
                     }).AsQueryable();
            return Task.FromResult(q);
        }
    }
}