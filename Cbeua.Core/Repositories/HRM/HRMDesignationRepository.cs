using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMDesignationRepository : GenericRepository<HRMDesignation>, IHRMDesignationRepository
    {
        private readonly AppDbContext _context;

        public HRMDesignationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMDesignationDTO>> GetQuerableList()
        {
            var q = (from designation in _context.HRMDesignations
                     join department in _context.HRMDepartments on designation.HRMDepartmentId equals department.Id
                     select new HRMDesignationDTO
                     {
                         Id = designation.Id,
                         HRMDepartmentId = designation.HRMDepartmentId,
                         DepartmentName = department.Name,
                         Name = designation.Name,
                         Description = designation.Description,
                         IsActive = designation.IsActive,
                         IsDeleted = designation.IsDeleted,
                         CreatedAt = designation.CreatedAt,
                         UpdatedAt = designation.UpdatedAt
                     }).AsQueryable();

            return Task.FromResult(q);
        }
    }
}