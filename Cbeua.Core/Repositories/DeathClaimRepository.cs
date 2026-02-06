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
                    join m in _context.Members on dc.MemberId equals m.MemberId
                    join s in _context.States on dc.StateId equals s.StateId into stateJoin
                    from s in stateJoin.DefaultIfEmpty()
                    join d in _context.Designations on dc.DesignationId equals d.DesignationId into designationJoin
                    from d in designationJoin.DefaultIfEmpty()
                    join year in _context.YearMasters on dc.YearOF equals year.YearOf
                    where !dc.IsDeleted
                    select new DeathClaimDTO
                    {
                        DeathClaimId = dc.DeathClaimId,
                        MemberId = dc.MemberId,
                        MemberName = m.Name, // Direct from member join
                        StaffNo = m.StaffNo, // Direct from member join
                        StateId = dc.StateId,
                        StateName = s != null ? s.Name : "",
                        DesignationId = dc.DesignationId,
                        DesignationName = d != null ? d.Name : "",
                        DeathDate = dc.DeathDate,
                        Nominee = dc.Nominee,
                        NomineeRelation = dc.NomineeRelation,
                        NomineeIDentity = dc.NomineeIDentity,
                        DDNO = dc.DDNO,
                        DDDATE = dc.DDDATE,
                        Amount = dc.Amount,
                        LastContribution = dc.LastContribution,
                        YearOF = dc.YearOF,
                        YearName = year.YearName,
                        IsDeleted = dc.IsDeleted
                    };
            return q;
        }
    }
}