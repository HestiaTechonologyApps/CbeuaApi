using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class DeathClaimRepository : GenericRepository<DeathClaim>, IDeathClaimRepository
    {
        private readonly AppDbContext _context;
        public DeathClaimRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<DeathClaimDTO> GetQueryableDeathClaims()
        {
            var q = from dc in _context.DeathClaims
                    select new DeathClaimDTO
                    {
                        DeathClaimId = dc.DeathClaimId,
                        MemberId = dc.MemberId,
                        StateId = dc.StateId,
                        DesignationId = dc.DesignationId,
                        DeathDate = dc.DeathDate,
                        Nominee = dc.Nominee,
                        NomineeRelation = dc.NomineeRelation,
                        NomineeIDentity = dc.NomineeIDentity,
                        DDNO = dc.DDNO,
                        DDDATE = dc.DDDATE,
                        Amount = dc.Amount,
                        LastContribution = dc.LastContribution,
                        YearOF = dc.YearOF

                    };
            return q;
        }
    }
}
