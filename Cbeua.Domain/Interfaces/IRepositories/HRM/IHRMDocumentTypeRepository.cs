using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;

namespace Cbeua.Domain.Interfaces.IRepositories.HRMS
{
    public interface IHRMDocumentTypeRepository : IGenericRepository<HRMDocumentType>
    {
        Task<IQueryable<HRMDocumentTypeDTO>> GetQuerableList();
    }
}