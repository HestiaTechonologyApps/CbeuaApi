using Cbeua.Core.Helpers;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var q = _repo.GetQueryableMemberById(id);
            return await q.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<MemberDTO> CreateAsync(Member member)
        {
            if (await _repo.IsStaffNoInUseAsync(member.StaffNo))
                throw new InvalidOperationException("Staff No already exists");

            if (member.OldStaffNo.HasValue
                && await _repo.IsStaffNoInUseAsync(member.OldStaffNo.Value))
                throw new InvalidOperationException("Staff No already exists");

            member.IsDeleted = false;
            await _repo.AddAsync(member);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<Member>(
               tableName: AuditTableName,
               action: "create",
               recordId: member.MemberId,
               oldEntity: null,
               newEntity: member,
               changedBy: "System"
            );
            return await ConvertMemberToDTO(member);
        }

        private async Task<MemberDTO> ConvertMemberToDTO(Member member)
        {
            MemberDTO memberDTO = new MemberDTO();
            memberDTO.MemberId = member.MemberId;
            memberDTO.StaffNo = member.StaffNo;
            memberDTO.OldStaffNo = member.OldStaffNo;
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
            memberDTO.IsDeleted = member.IsDeleted;
            return memberDTO;
        }

        private Member CloneMember(Member member)
        {
            return new Member
            {
                MemberId = member.MemberId,
                StaffNo = member.StaffNo,
                OldStaffNo = member.OldStaffNo,
                DesignationId = member.DesignationId,
                CategoryId = member.CategoryId,
                BranchId = member.BranchId,
                Name = member.Name,
                GenderId = member.GenderId,
                Dob = member.Dob,
                Doj = member.Doj,
                DojtoScheme = member.DojtoScheme,
                StatusId = member.StatusId,
                IsRegCompleted = member.IsRegCompleted,
                CreatedByUserId = member.CreatedByUserId,
                CreatedDate = member.CreatedDate,
                ModifiedByUserId = member.ModifiedByUserId,
                ModifiedDate = member.ModifiedDate,
                Nominee = member.Nominee,
                ProfileImageSrc = member.ProfileImageSrc,
                NomineeRelation = member.NomineeRelation,
                NomineeIDentity = member.NomineeIDentity,
                UnionMember = member.UnionMember,
                TotalRefund = member.TotalRefund,
                IsDeleted = member.IsDeleted
            };
        }

        public async Task<bool> UpdateAsync(Member member)
        {
            var oldentity = await _repo.GetByIdAsync(member.MemberId);
            if (oldentity == null || oldentity.IsDeleted) return false;

            if (await _repo.IsStaffNoInUseAsync(member.StaffNo, member.MemberId))
                throw new InvalidOperationException("Staff No already exists");

            if (member.OldStaffNo.HasValue
                && await _repo.IsStaffNoInUseAsync(member.OldStaffNo.Value, member.MemberId))
                throw new InvalidOperationException("Staff No already exists");

            _repo.Detach(oldentity);
            _repo.Update(member);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<Member>(
               tableName: AuditTableName,
               action: "update",
               recordId: member.MemberId,
               oldEntity: oldentity,
               newEntity: member,
               changedBy: "System"
            );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var member = await _repo.GetByIdAsync(id);
            if (member == null || member.IsDeleted) return false;

            var oldEntity = CloneMember(member);

            member.IsDeleted = true;
            _repo.Update(member);

            await _auditRepository.LogAuditAsync<Member>(
               tableName: AuditTableName,
               action: "delete",
               recordId: member.MemberId,
               oldEntity: oldEntity,
               newEntity: member,
               changedBy: "System"
            );
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<CustomApiResponse> UpdateProfilePicAsync(int MemberId, string ProfileImageSrc)
        {
            var member = await _repo.GetByIdAsync(MemberId);
            if (member == null || member.IsDeleted)
                return new CustomApiResponse { IsSucess = false, Error = "Member not found", StatusCode = 404 };

            member.ProfileImageSrc = ProfileImageSrc;
            _repo.Update(member);
            await _repo.SaveChangesAsync();

            return new CustomApiResponse { IsSucess = true, Value = ProfileImageSrc, StatusCode = 200 };
        }

        /// <summary>
        /// ✅ FIXED PAGINATION - Latest members first + In-memory search
        /// </summary>
        public async Task<PagedResult<MemberDTO>> GetPagedMembersAsync(MemberPaginationParams parameters)
        {
            // Start with queryable
            var query = _repo.GetQueryableMember();

            // ✅ Apply SQL-compatible filters BEFORE .ToList()
            if (parameters.BranchId.HasValue && parameters.BranchId.Value > 0)
                query = query.Where(m => m.BranchId == parameters.BranchId.Value);

            if (parameters.CategoryId.HasValue && parameters.CategoryId.Value > 0)
                query = query.Where(m => m.CategoryId == parameters.CategoryId.Value);

            if (parameters.DesignationId.HasValue && parameters.DesignationId.Value > 0)
                query = query.Where(m => m.DesignationId == parameters.DesignationId.Value);

            if (parameters.StatusId.HasValue && parameters.StatusId.Value > 0)
                query = query.Where(m => m.StatusId == parameters.StatusId.Value);

            if (parameters.GenderId.HasValue && parameters.GenderId.Value >= 0)
                query = query.Where(m => m.GenderId == parameters.GenderId.Value);

            // ✅ Execute query and bring to memory
            var allMembers = query.ToList();

            // ✅ Apply search in-memory
            IEnumerable<MemberDTO> filteredMembers = allMembers;

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                var searchLower = parameters.SearchTerm.ToLower().Trim();

                filteredMembers = allMembers.Where(m =>
                    (m.MemberId.ToString().Contains(searchLower)) ||
                    (m.StaffNo.ToString().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(m.Name) && m.Name.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(m.BranchName) && m.BranchName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(m.Categoryname) && m.Categoryname.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(m.DesignationName) && m.DesignationName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(m.Nominee) && m.Nominee.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(m.Status) && m.Status.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(m.Gender) && m.Gender.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(m.DpCode) && m.DpCode.ToLower().Contains(searchLower))
                );
            }

            // ✅ Apply sorting in-memory
            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                var sortBy = parameters.SortBy.ToLower();

                filteredMembers = sortBy switch
                {
                    "memberid" => parameters.SortDescending
                        ? filteredMembers.OrderByDescending(m => m.MemberId)
                        : filteredMembers.OrderBy(m => m.MemberId),
                    "name" => parameters.SortDescending
                        ? filteredMembers.OrderByDescending(m => m.Name)
                        : filteredMembers.OrderBy(m => m.Name),
                    "staffno" => parameters.SortDescending
                        ? filteredMembers.OrderByDescending(m => m.StaffNo)
                        : filteredMembers.OrderBy(m => m.StaffNo),
                    "branch" or "branchname" => parameters.SortDescending
                        ? filteredMembers.OrderByDescending(m => m.BranchName)
                        : filteredMembers.OrderBy(m => m.BranchName),
                    "category" or "categoryname" => parameters.SortDescending
                        ? filteredMembers.OrderByDescending(m => m.Categoryname)
                        : filteredMembers.OrderBy(m => m.Categoryname),
                    "designation" or "designationname" => parameters.SortDescending
                        ? filteredMembers.OrderByDescending(m => m.DesignationName)
                        : filteredMembers.OrderBy(m => m.DesignationName),
                    "doj" => parameters.SortDescending
                        ? filteredMembers.OrderByDescending(m => m.Doj ?? DateTime.MinValue)
                        : filteredMembers.OrderBy(m => m.Doj ?? DateTime.MinValue),
                    "dob" => parameters.SortDescending
                        ? filteredMembers.OrderByDescending(m => m.Dob ?? DateTime.MinValue)
                        : filteredMembers.OrderBy(m => m.Dob ?? DateTime.MinValue),
                    "status" => parameters.SortDescending
                        ? filteredMembers.OrderByDescending(m => m.Status)
                        : filteredMembers.OrderBy(m => m.Status),
                    "gender" => parameters.SortDescending
                        ? filteredMembers.OrderByDescending(m => m.Gender)
                        : filteredMembers.OrderBy(m => m.Gender),
                    _ => parameters.SortDescending
                        ? filteredMembers.OrderByDescending(m => m.MemberId)
                        : filteredMembers.OrderBy(m => m.MemberId)
                };
            }
            else
            {
                // ✅ DEFAULT SORT: Latest members first (highest MemberId first)
                filteredMembers = filteredMembers.OrderByDescending(m => m.MemberId);
            }

            // ✅ Get total count before pagination
            var totalRecords = filteredMembers.Count();

            // ✅ Apply pagination
            var pageNumber = parameters.PageNumber;
            var pageSize = parameters.PageSize;

            List<MemberDTO> pagedData;

            if (parameters.GetAll)
            {
                pagedData = filteredMembers.ToList();
            }
            else
            {
                pagedData = filteredMembers
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }

            // ✅ Return paginated result
            return new PagedResult<MemberDTO>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = parameters.GetAll ? totalRecords : pageSize
            };
        }
        // MemberService.cs
        public async Task<List<StatusDTO>> GetDistinctMemberStatusesAsync()
        {
            return await _repo.GetDistinctMemberStatusesAsync();
        }
    }
}