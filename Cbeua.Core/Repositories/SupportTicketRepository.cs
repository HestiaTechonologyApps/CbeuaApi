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
    public class SupportTicketRepository : GenericRepository<SupportTicket>, ISupportTicketRepository
    {
        private readonly AppDbContext _context;
        public SupportTicketRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<SupportTicketDTO> GetQueryableSupportTickets()
        {
            var q = from st in _context.SupportTickets
                    select new SupportTicketDTO
                    {
                        SupportTicketId = st.SupportTicketId,
                        SupportTicketNum = st.SupportTicketNum,
                        Description = st.Description,
                        Priority = st.Priority,
                        Duration = st.Duration,
                        DeveloperRemark = st.DeveloperRemark,
                        isApproved = st.isApproved,
                        ApprovedByUserId = st.ApprovedByUserId,
                        ApprovedDate = st.ApprovedDate

                        
                    };
            return q;
        }
    }
}
