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
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        private readonly AppDbContext _context;
        public BranchRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<BranchDTO> GetQuerableBranchList()
        {
            var q = from b in _context.Branches
                    select new BranchDTO
                    {
                        BranchId = b.BranchId,
                        DpCode = b.DpCode,
                        Name = b.Name,
                        Address1 = b.Address1,
                        Address2 = b.Address2,
                        Address3 = b.Address3,
                        District = b.District,
                        Status = b.Status,
                        CircleId = b.CircleId,
                        StateId = b.StateId,
                        IsRegCompleted = b.IsRegCompleted
                    };
            return q;
        }
    }
}
