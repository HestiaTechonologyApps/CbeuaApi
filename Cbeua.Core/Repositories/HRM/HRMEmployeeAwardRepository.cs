using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMEmployeeAwardRepository : GenericRepository<HrmEmployeeAward>, IHRMEmployeeAwardRepository
    {
        private readonly AppDbContext _context;

        public HRMEmployeeAwardRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMEmployeeAwardDTO>> GetQuerableList()
        {
            var q = (from award in _context.HrmEmployeeAwards
                     join emp in _context.HRMEmployees on award.HRMEmployeeId equals emp.Id
                     join awardType in _context.HRMAwardTypes on award.HrmAwardTypeId equals awardType.Id
                     select new HRMEmployeeAwardDTO
                     {
                         Id = award.Id,
                         HRMEmployeeId = award.HRMEmployeeId,
                         EmployeeName = emp.FullName,
                         EmployeeCode = emp.EmployeeCode,
                         HrmAwardTypeId = award.HrmAwardTypeId,
                         AwardTypeName = awardType.Name,
                         AwardDate = award.AwardDate,
                         Gift = award.Gift,
                         Description = award.Description,
                         MonetaryValue = award.MonetaryValue,
                         IsActive = award.IsActive,
                         IsDeleted = award.IsDeleted,
                         CreatedAt = award.CreatedAt,
                         UpdatedAt = award.UpdatedAt
                     }).AsQueryable();

            return Task.FromResult(q);
        }
    }
}