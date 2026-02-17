using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMDepartmentRepository : GenericRepository<HRMDepartment>, IHRMDepartmentRepository
    {
        private readonly AppDbContext _context;

        public HRMDepartmentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMDepartmentDTO>> GetQuerableList()
        {
            var q = (from dept in _context.HRMDepartments
                     join branch in _context.HRMBranches on dept.HRMBranchId equals branch.Id
                     select new HRMDepartmentDTO
                     {
                         Id = dept.Id,
                         HRMBranchId = dept.HRMBranchId,
                         BranchName = branch.Name,
                         Name = dept.Name,
                         Description = dept.Description,
                         IsActive = dept.IsActive,
                         IsDeleted = dept.IsDeleted,
                         CreatedAt = dept.CreatedAt,
                         UpdatedAt = dept.UpdatedAt
                     }).AsQueryable();

            return Task.FromResult(q);
        }
    }
}