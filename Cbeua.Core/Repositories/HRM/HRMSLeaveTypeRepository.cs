using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMSLeaveTypeRepository : GenericRepository<HRMSLeaveType>, IHRMSLeaveTypeRepository
    {
        private readonly AppDbContext _context;

        public HRMSLeaveTypeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMSLeaveTypeDTO>> GetQuerableList()
        {
            var q = (from lt in _context.HRMSLeaveTypes
                     select new HRMSLeaveTypeDTO
                     {
                         Id = lt.Id,
                         Name = lt.Name,
                         Description = lt.Description,
                         MaxDaysAllowed = lt.MaxDaysAllowed,
                         IsPaid = lt.IsPaid,
                         CarryForward = lt.CarryForward,
                         CarryForwardLimit = lt.CarryForwardLimit,
                         ApplicableGender = lt.ApplicableGender,
                         RequiresDocument = lt.RequiresDocument,
                         NoticeDaysRequired = lt.NoticeDaysRequired,
                         IsActive = lt.IsActive,
                         IsDeleted = lt.IsDeleted,
                         CreatedAt = lt.CreatedAt,
                         UpdatedAt = lt.UpdatedAt
                     }).AsQueryable();

            return Task.FromResult(q);
        }
    }
}