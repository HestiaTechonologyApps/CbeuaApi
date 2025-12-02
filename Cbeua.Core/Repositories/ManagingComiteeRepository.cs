using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class ManagingComiteeRepository : GenericRepository<ManagingComitee>,IManagingComiteeRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public ManagingComiteeRepository(AppDbContext context, ICurrentUserService currentUser) : base(context)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public IQueryable<ManagingComiteeDTO> GetQueryableManagingComitteeList()
        {
            int companyId = Convert.ToInt32(_currentUser.CompanyId);

            var query =
                from m in _context.ManagingComitees
                join cmp in _context.Companies on m.CompanyId equals cmp.CompanyId
                where m.CompanyId == companyId
                select new ManagingComiteeDTO
                {
                    ManagingComiteeId = m.ManagingComiteeId,
                    ManagingComitteeName = m.ManagingComitteeName,
                    Position = m.Position,
                    Description1 = m.Description1,
                    Description2 = m.Description2,
                    imageLocation = m.imageLocation,
                    order = m.order,
                    CompanyId = m.CompanyId,
                    CompanyName = cmp.ComapanyName
                };

            return query;
        }


        public async Task<IEnumerable<ManagingComiteeDTO>> GetAllAsync()
        {
            return await GetQueryableManagingComitteeList().ToListAsync();
        }
        public async Task<ManagingComiteeDTO> GetByIdDTOAsync(int id)
        {
            var managingComitee = await GetQueryableManagingComitteeList()
                .FirstOrDefaultAsync(m => m.ManagingComiteeId == id);
            return managingComitee;
        }
    }
}
