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
    public class UserTypeRepository : GenericRepository<UserType>, IUserTypeRepository
    {
        private readonly AppDbContext _context;
        public UserTypeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<UserTypeDTO> GetQueryableUserTypes()
        {
            var q = from ut in _context.UserTypes
                    select new UserTypeDTO
                    {
                        UserTypeId = ut.UserTypeId,
                        Abbreviation = ut.Abbreviation,
                        Description = ut.Description

                    };
            return q;
        }
    }
}
