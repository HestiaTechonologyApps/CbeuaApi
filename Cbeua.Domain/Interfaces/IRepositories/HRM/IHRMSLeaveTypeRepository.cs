using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;

namespace Cbeua.Domain.Interfaces.IRepositories.HRMS
{
    public interface IHRMSLeaveTypeRepository : IGenericRepository<HRMSLeaveType>
    {
        Task<IQueryable<HRMSLeaveTypeDTO>> GetQuerableList();
    }
}