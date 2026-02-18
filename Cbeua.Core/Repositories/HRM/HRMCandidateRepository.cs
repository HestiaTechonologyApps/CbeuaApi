using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMCandidateRepository : GenericRepository<HRMSCandidate>, IHRMCandidateRepository
    {
        private readonly AppDbContext _context;

        public HRMCandidateRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMCandidateDTO>> GetQuerableList()
        {
            var q = (from candidate in _context.HRMSCandidates
                     select new HRMCandidateDTO
                     {
                         Id = candidate.Id,
                         Email = candidate.Email,
                         Phone = candidate.Phone,
                         Address = candidate.Address,
                         City = candidate.City,
                         State = candidate.State,
                         Country = candidate.Country,
                         ZipCode = candidate.ZipCode,
                         ExperienceInYears = candidate.ExperienceInYears,
                         CurrentSalary = candidate.CurrentSalary,
                         ExpectedSalary = candidate.ExpectedSalary,
                         NoticePeriod = candidate.NoticePeriod,
                         IsActive = candidate.IsActive,
                         IsDeleted = candidate.IsDeleted,
                         CreatedAt = candidate.CreatedAt,
                         UpdatedAt = candidate.UpdatedAt
                     }).AsQueryable();

            return Task.FromResult(q);
        }
    }
}