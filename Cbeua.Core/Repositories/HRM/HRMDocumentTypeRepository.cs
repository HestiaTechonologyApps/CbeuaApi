using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMDocumentTypeRepository : GenericRepository<HRMDocumentType>, IHRMDocumentTypeRepository
    {
        private readonly AppDbContext _context;

        public HRMDocumentTypeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMDocumentTypeDTO>> GetQuerableList()
        {
            var q = (from docType in _context.HRMDocumentTypes
                     select new HRMDocumentTypeDTO
                     {
                         Id = docType.Id,
                         Name = docType.Name,
                         Description = docType.Description,
                         IsActive = docType.IsActive,
                         IsDeleted = docType.IsDeleted,
                         CreatedAt = docType.CreatedAt,
                         UpdatedAt = docType.UpdatedAt
                     }).AsQueryable();

            return Task.FromResult(q);
        }
    }
}