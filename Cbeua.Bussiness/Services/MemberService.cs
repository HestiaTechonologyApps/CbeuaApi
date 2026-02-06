using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Cbeua.Core.Helpers;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "MEMBER";

        public MemberService(IMemberRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<MemberDTO>> GetAllAsync()
        {
            return _repo.GetQueryableMember().ToList();
        }

        public async Task<MemberDTO?> GetByIdAsync(int id)
        {
            var q = _repo.GetQueryableMember();
            var memeber = q.Where(u => u.MemberId == id).FirstOrDefault();
            return memeber;
        }

        public async Task<MemberDTO> CreateAsync(Member member)
        {
            await _repo.AddAsync(member);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<Member>(
               tableName: AuditTableName,
               action: "create",
               recordId: member.MemberId,
               oldEntity: null,
               newEntity: member,
               changedBy: "System" // Replace with actual user info
               );
            return await ConvertMemberToDTO(member);
        }

        private async Task<MemberDTO> ConvertMemberToDTO(Member member)
        {
            MemberDTO memberDTO = new MemberDTO();
            memberDTO.MemberId = member.MemberId;
            memberDTO.StaffNo = member.StaffNo;
            memberDTO.DesignationId = member.DesignationId;
            memberDTO.CategoryId = member.CategoryId;
            memberDTO.BranchId = member.BranchId;
            memberDTO.Name = member.Name;
            memberDTO.GenderId = member.GenderId;

            memberDTO.Dob = member.Dob;
            memberDTO.Doj = member.Doj;
            memberDTO.DojtoScheme = member.DojtoScheme;
            memberDTO.StatusId = member.StatusId;
            memberDTO.IsRegCompleted = member.IsRegCompleted;
            memberDTO.CreatedByUserId = member.CreatedByUserId;
            memberDTO.CreatedDate = member.CreatedDate;
            memberDTO.ModifiedByUserId = member.ModifiedByUserId;
            memberDTO.ModifiedDate = member.ModifiedDate;
            memberDTO.Nominee = member.Nominee;
            memberDTO.ProfileImageSrc = member.ProfileImageSrc;
            memberDTO.NomineeRelation = member.NomineeRelation;
            memberDTO.UnionMember = member.UnionMember;
            memberDTO.TotalRefund = member.TotalRefund;
            return memberDTO;
        }

        public async Task<bool> UpdateAsync(Member member)
        {
            var oldentity = await _repo.GetByIdAsync(member.MemberId);
            _repo.Detach(oldentity);
            _repo.Update(member);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<Member>(
               tableName: AuditTableName,
               action: "update",
               recordId: member.MemberId,
               oldEntity: oldentity,
               newEntity: member,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var member = await _repo.GetByIdAsync(id);
            if (member == null) return false;
            _repo.Delete(member);
            await _auditRepository.LogAuditAsync<Member>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: member.MemberId,
               oldEntity: member,
               newEntity: member,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<CustomApiResponse> UpdateProfilePicAsync(int MemberId, string ProfileImageSrc)
        {
            var member = await _repo.GetByIdAsync(MemberId);
            if (member == null)
                return new CustomApiResponse { IsSucess = false, Error = "Member not found", StatusCode = 404 };

            member.ProfileImageSrc = ProfileImageSrc;
            _repo.Update(member);
            await _repo.SaveChangesAsync();

            return new CustomApiResponse { IsSucess = true, Value = ProfileImageSrc, StatusCode = 200 };
        }

        // NEW METHOD - Pagination Implementation
        public async Task<PagedResult<MemberDTO>> GetPagedMembersAsync(MemberPaginationParams parameters)
        {
            var query = _repo.GetQueryableMember();

            // Apply filters
            if (parameters.BranchId.HasValue)
                query = query.Where(m => m.BranchId == parameters.BranchId.Value);

            if (parameters.CategoryId.HasValue)
                query = query.Where(m => m.CategoryId == parameters.CategoryId.Value);

            if (parameters.DesignationId.HasValue)
                query = query.Where(m => m.DesignationId == parameters.DesignationId.Value);

            if (parameters.StatusId.HasValue)
                query = query.Where(m => m.StatusId == parameters.StatusId.Value);

            if (parameters.GenderId.HasValue)
                query = query.Where(m => m.GenderId == parameters.GenderId.Value);

            // Apply search
            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                query = query.ApplySearch(
                    parameters.SearchTerm,
                    m => m.Name,
                    m => m.StaffNo.ToString(),
                    m => m.BranchName,
                    m => m.Categoryname,
                    m => m.DesignationName,
                    m => m.Nominee
                );
            }

            // Apply sorting
            var sortMappings = new Dictionary<string, Expression<Func<MemberDTO, object>>>
            {
                { "name", m => m.Name },
                { "staffno", m => m.StaffNo },
                { "branch", m => m.BranchName },
                { "category", m => m.Categoryname },
                { "designation", m => m.DesignationName },
                { "doj", m => m.Doj ?? DateTime.MinValue },
                { "status", m => m.Status }
            };

            query = query.ApplySort(
                parameters.SortBy ?? "name",
                parameters.SortDescending,
                sortMappings
            );

            // Apply pagination and return result
            return await query.ToPaginatedListAsync(parameters.PageNumber, parameters.PageSize);
        }
    }
}