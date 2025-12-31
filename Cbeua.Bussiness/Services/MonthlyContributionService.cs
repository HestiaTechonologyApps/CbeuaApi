using Cbeua.Domain.DTO;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices.Common;

namespace Cbeua.Bussiness.Services
{
    public class MonthlyContributionService : IMonthlyContributionService
    {
        private readonly IMonthlyContributionService  _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "DESIGNATION";
        public MonthlyContributionService(IMonthlyContributionService repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }
        public Task<List<IndividualContrbutionDTO>> GetIndividualContrbution(int MemeberId)
        {
            ///var q = from account in _co
            return  _repo.GetIndividualContrbution(MemeberId);
                     
        }
    }
}
