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
    public class UserRoleRightRepository : GenericRepository<UserRoleRight>, IUserRoleRightRepository
    {
        private readonly AppDbContext _context;
        public UserRoleRightRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<UserRoleRight> GetQueryableUserRoleRights()
        {
            var q = from urr in _context.UserRoleRights
                    select new UserRoleRight
                    {
                        UserRoleRightId = urr.UserRoleRightId,
                        ControllerName = urr.ControllerName,
                        ActionName = urr.ActionName,
                        UserTypeID = urr.UserTypeID
                    };
            return q;
        }
    }
}
