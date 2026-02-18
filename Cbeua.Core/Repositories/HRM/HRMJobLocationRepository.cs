using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMJobLocationRepository : GenericRepository<HRMSJobLocation>, IHRMJobLocationRepository
    {
        private readonly AppDbContext _context;
        public HRMJobLocationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMJobLocationDTO>> GetQuerableList()
        {
            var q = (from jobLocation in _context.HRMSJobLocations
                     select new HRMJobLocationDTO
                     {
                         Id = jobLocation.Id,
                         Name = jobLocation.Name,
                         Address = jobLocation.Address,
                         IsRemote = jobLocation.IsRemote,
                         City = jobLocation.City,
                         State = jobLocation.State,
                         Country = jobLocation.Country,
                         PostalCode = jobLocation.PostalCode,
                         IsActive = jobLocation.IsActive,
                         IsDeleted = jobLocation.IsDeleted,
                         CreatedAt = jobLocation.CreatedAt,
                         UpdatedAt = jobLocation.UpdatedAt
                     }).AsQueryable();
            return Task.FromResult(q);
        }
    }
}