using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMBranchRepository : GenericRepository<HRMBranch>, IHRMBranchRepository
    {
        private readonly AppDbContext _context;

        public HRMBranchRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMBranchDTO>> GetQuerableList()
        {
            var q = (from branch in _context.HRMBranches
                     select new HRMBranchDTO
                     {
                         Id = branch.Id,
                         Name = branch.Name,
                         Address = branch.Address,
                         State = branch.State,
                         Country = branch.Country,
                         ZiPcode = branch.ZiPcode,
                         Phone = branch.Phone,
                         Email = branch.Email,
                         IsActive = branch.IsActive,
                         IsDeleted = branch.IsDeleted,
                         CreatedAt = branch.CreatedAt,
                         UpdatedAt = branch.UpdatedAt
                     }).AsQueryable();

            return Task.FromResult(q);
        }
    }
}