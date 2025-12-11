using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
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
            List<MemberDTO> memberDTOs = new List<MemberDTO>();
            var members = await _repo.GetAllAsync();

            foreach (var member in members)
            {
                MemberDTO memberDTO = await ConvertMemberToDTO(member);
                memberDTOs.Add(memberDTO);


            }

            return memberDTOs;
        }

        public async Task<MemberDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var memberDTO = await ConvertMemberToDTO(q);
            return memberDTO;
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
            memberDTO.ImageId = member.ImageId;
            memberDTO.Dob = member.Dob;
            memberDTO.Doj = member.Doj; 
            memberDTO.DojtoScheme = member.DojtoScheme;
            memberDTO.StatusId = member.StatusId;
            memberDTO.IsRegCompleted = member.IsRegCompleted;
            memberDTO.CreatedByUserId = member.CreatedByUserId;
            memberDTO.CreatedDate = member.CreatedDate;
            memberDTO.ModifiedByUserId = member.ModifiedByUserId;
            memberDTO.ModifiedDate = member.ModifiedDate;
            memberDTO.Nominee=member.Nominee;
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
    }
}
