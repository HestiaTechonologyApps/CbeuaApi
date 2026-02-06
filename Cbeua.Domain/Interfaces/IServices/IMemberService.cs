using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IMemberService
    {
        Task<List<MemberDTO>> GetAllAsync();
        Task<MemberDTO?> GetByIdAsync(int id);
        Task<MemberDTO> CreateAsync(Member member);
        Task<bool> UpdateAsync(Member member);
        Task<bool> DeleteAsync(int id);
        Task<CustomApiResponse> UpdateProfilePicAsync(int MemberId, string ProfileImageSrc);

        // Add this new method for pagination
        Task<PagedResult<MemberDTO>> GetPagedMembersAsync(MemberPaginationParams parameters);
    }
}