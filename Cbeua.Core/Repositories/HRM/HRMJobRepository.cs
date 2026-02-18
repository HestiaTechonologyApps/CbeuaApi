using Cbeua.Domain.DTO.HRMS;
using Cbeua.Domain.Entities.HRMS;
using Cbeua.Domain.Interfaces.IRepositories.HRMS;
using Cbeua.InfraCore.Data;

namespace Cbeua.Core.Repositories.HRMS
{
    public class HRMJobRepository : GenericRepository<HRMSJob>, IHRMJobRepository
    {
        private readonly AppDbContext _context;
        public HRMJobRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IQueryable<HRMJobDTO>> GetQuerableList()
        {
            var q = (from job in _context.HRMSJobs
                     select new HRMJobDTO
                     {
                         Id = job.Id,
                         JobTitle = job.JobTitle,
                         Location = job.Location,
                         Branch = job.Branch,
                         Department = job.Department,
                         StartDate = job.StartDate,
                         EndDate = job.EndDate,
                         ExistingLink = job.ExistingLink,
                         NumberOfOpenings = job.NumberOfOpenings,
                         MinimumExperienceYears = job.MinimumExperienceYears,
                         MaximumExperienceYears = job.MaximumExperienceYears,
                         MinimumSalary = job.MinimumSalary,
                         MaximumSalary = job.MaximumSalary,
                         JobDescription = job.JobDescription,
                         JobRequrement = job.JobRequrement,
                         JobBenefits = job.JobBenefits,
                         IsActive = job.IsActive,
                         IsDeleted = job.IsDeleted,
                         CreatedAt = job.CreatedAt,
                         UpdatedAt = job.UpdatedAt
                     }).AsQueryable();
            return Task.FromResult(q);
        }
    }
}