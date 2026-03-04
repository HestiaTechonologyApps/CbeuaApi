using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMSLeaveApplicationRepository : GenericRepository<HRMSLeaveApplication>, IHRMSLeaveApplicationRepository
    {
        private readonly AppDbContext _context;

        public HRMSLeaveApplicationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMSLeaveApplicationDTO>> GetQuerableList()
        {
            var q = (from la in _context.HRMSLeaveApplications
                     join emp in _context.HRMEmployees on la.HRMEmployeeId equals emp.Id
                     join lt in _context.HRMSLeaveTypes on la.HRMSLeaveTypeId equals lt.Id
                     join reviewer in _context.HRMEmployees on la.ReviewedBy equals reviewer.Id into reviewerGroup
                     from reviewer in reviewerGroup.DefaultIfEmpty()
                     select new HRMSLeaveApplicationDTO
                     {
                         Id = la.Id,
                         HRMEmployeeId = la.HRMEmployeeId,
                         EmployeeName = emp.FullName,
                         HRMSLeaveTypeId = la.HRMSLeaveTypeId,
                         LeaveTypeName = lt.Name,
                         FromDate = la.FromDate,
                         ToDate = la.ToDate,
                         TotalDays = la.TotalDays,
                         DayType = la.DayType,
                         Reason = la.Reason,
                         Status = la.Status,
                         AppliedOn = la.AppliedOn,
                         DocumentUrl = la.DocumentUrl,
                         ReviewedBy = la.ReviewedBy,
                         ReviewedByName = reviewer != null ? reviewer.FullName : "",
                         ReviewedOn = la.ReviewedOn,
                         ReviewerRemarks = la.ReviewerRemarks,
                         IsActive = la.IsActive,
                         IsDeleted = la.IsDeleted,
                         CreatedAt = la.CreatedAt,
                         UpdatedAt = la.UpdatedAt
                     }).AsQueryable();

            return Task.FromResult(q);
        }
    }
}