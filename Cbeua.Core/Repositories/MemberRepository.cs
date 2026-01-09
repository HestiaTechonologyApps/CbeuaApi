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
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        private readonly AppDbContext _context;
        public MemberRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<MemberDTO> GetQueryableMember()
        {
            var q = from m in _context.Members
                    join c in _context.Categories on m.CategoryId equals c.CategoryId
                    join d in _context.Designations on m.DesignationId equals d.DesignationId 
                    join b in _context.Branches on m.BranchId equals b.BranchId
                    join s in _context.statuses on m.StatusId equals s.StatusId 
                    select new MemberDTO
                    {
                        MemberId = m.MemberId,
                        StaffNo = m.StaffNo,
                        DesignationId = m.DesignationId,
                        CategoryId = m.CategoryId,
                        BranchId = m.BranchId,
                        Name = m.Name,
                        GenderId = m.GenderId,

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
                        DpCode=b.DpCode.ToString (),
                        BranchName=b.Name , 
                        Status=s.Name,
                        Categoryname = c.Name,
                        DesignationName=d.Name


                    };
            return q;
        }
    }
}
