using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;

namespace Cbeua.Domain.Interfaces.IRepositories.HRMS
{
    public interface IHRMSLeaveApplicationRepository : IGenericRepository<HRMSLeaveApplication>
    {
        Task<IQueryable<HRMSLeaveApplicationDTO>> GetQuerableList();
    }
}