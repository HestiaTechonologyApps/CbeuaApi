using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        private readonly AppDbContext _context;
        public MemberRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<MemberDTO> GetQueryableMember()
        {
            var q = from m in _context.Members.AsNoTracking()
                    join c in _context.Categories.AsNoTracking() on m.CategoryId equals c.CategoryId
                    join d in _context.Designations.AsNoTracking() on m.DesignationId equals d.DesignationId 
                    join b in _context.Branches.AsNoTracking() on m.BranchId equals b.BranchId
                    join s in _context.statuses.AsNoTracking() on m.StatusId equals s.StatusId
                    where !m.IsDeleted
                    select new MemberDTO
                    {
                        MemberId = m.MemberId,
                        StaffNo = m.StaffNo,
                        OldStaffNo = m.OldStaffNo,
                        DesignationId = m.DesignationId,
                        CategoryId = m.CategoryId,
                        BranchId = m.BranchId,
                        Name = m.Name,
                        GenderId = m.GenderId,
                        Gender = m.GenderId == 0 ? "Male" : m.GenderId == 1 ? "Female" : "Others",

                        Dob = m.Dob,
                        Doj = m.Doj,
                        DojtoScheme = m.DojtoScheme,
                        StatusId = m.StatusId,
                        IsRegCompleted = m.IsRegCompleted,
                        CreatedByUserId = m.CreatedByUserId,
                        CreatedDate = m.CreatedDate,
                        ModifiedByUserId = m.ModifiedByUserId,
                        ModifiedDate = m.ModifiedDate,
                        Nominee = m.Nominee,
                        ProfileImageSrc = m.ProfileImageSrc,
                        NomineeRelation = m.NomineeRelation,
                        NomineeIDentity = m.NomineeIDentity,
                        UnionMember = m.UnionMember,
                        TotalRefund = m.TotalRefund,
                        IsDeleted = m.IsDeleted,
                        DpCode =b.DpCode.ToString (),
                        BranchName=b.Name , 
                        Status=s.Name,
                        Categoryname = c.Name,
                        DesignationName=d.Name


                    };
            return q;
        }
        public IQueryable<MemberDTO> GetQueryableMemberById(int memberId)
        {
            var q = from m in _context.Members.AsNoTracking()
                    join c in _context.Categories.AsNoTracking() on m.CategoryId equals c.CategoryId
                    join d in _context.Designations.AsNoTracking() on m.DesignationId equals d.DesignationId
                    join b in _context.Branches.AsNoTracking() on m.BranchId equals b.BranchId
                    join s in _context.statuses.AsNoTracking() on m.StatusId equals s.StatusId
                    where !m.IsDeleted && m.MemberId == memberId
                    select new MemberDTO
                    {
                        MemberId = m.MemberId,
                        StaffNo = m.StaffNo,
                        OldStaffNo = m.OldStaffNo,
                        DesignationId = m.DesignationId,
                        CategoryId = m.CategoryId,
                        BranchId = m.BranchId,
                        Name = m.Name,
                        GenderId = m.GenderId,
                        Gender = m.GenderId == 0 ? "Male" : m.GenderId == 1 ? "Female" : "Others",
                        Dob = m.Dob,
                        Doj = m.Doj,
                        DojtoScheme = m.DojtoScheme,
                        StatusId = m.StatusId,
                        IsRegCompleted = m.IsRegCompleted,
                        CreatedByUserId = m.CreatedByUserId,
                        CreatedDate = m.CreatedDate,
                        ModifiedByUserId = m.ModifiedByUserId,
                        ModifiedDate = m.ModifiedDate,
                        Nominee = m.Nominee,
                        ProfileImageSrc = m.ProfileImageSrc,
                        NomineeRelation = m.NomineeRelation,
                        NomineeIDentity = m.NomineeIDentity,
                        UnionMember = m.UnionMember,
                        TotalRefund = m.TotalRefund,
                        IsDeleted = m.IsDeleted,
                        DpCode = b.DpCode.ToString(),
                        BranchName = b.Name,
                        Status = s.Name,
                        Categoryname = c.Name,
                        DesignationName = d.Name
                    };
            return q;
        }

        public IQueryable<MemberLookupDTO> GetMemberLookup(int branchId = 0)
        {
            var q = from m in _context.Members.AsNoTracking()
                    join b in _context.Branches.AsNoTracking() on m.BranchId equals b.BranchId
                    where !m.IsDeleted
                    select new { m, b };

            if (branchId != 0)
                q = q.Where(x => x.m.BranchId == branchId);
            return q.Select(x => new MemberLookupDTO
            {
                MemberId = x.m.MemberId,
                StaffNo = x.m.StaffNo,
                MemberName = x.m.Name,
                BranchName = x.b.Name
            });
        }

        public async Task<bool> IsStaffNoInUseAsync(int staffNo, int excludeMemberId = 0)
        {
            return await _context.Members.AsNoTracking()
                .AnyAsync(m => !m.IsDeleted
                    && m.MemberId != excludeMemberId
                    && (m.StaffNo == staffNo || m.OldStaffNo == staffNo));
        }
       
        public async Task<List<StatusFilterDTO>> GetDistinctMemberStatusesAsync()
        {
            return await _context.Members.AsNoTracking()
                .Where(m => !m.IsDeleted)
                .Join(_context.statuses.AsNoTracking(),
                      m => m.StatusId,
                      s => s.StatusId,
                      (m, s) => new StatusFilterDTO { StatusId = s.StatusId, Name = s.Name })
                .Distinct()
                .ToListAsync();
        }
    }
}