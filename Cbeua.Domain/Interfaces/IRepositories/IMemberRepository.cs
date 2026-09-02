using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        IQueryable<MemberDTO> GetQueryableMember();
        IQueryable<MemberLookupDTO> GetMemberLookup(int branchId = 0);
        IQueryable<MemberDTO> GetQueryableMemberById(int memberId);
        Task<bool> IsStaffNoInUseAsync(int staffNo, int excludeMemberId = 0);
        Task<List<StatusFilterDTO>> GetDistinctMemberStatusesAsync();
    }
}
