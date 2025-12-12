using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface ISupportTicketService
    {
        Task<List<SupportTicketDTO>> GetAllAsync();
        Task<SupportTicketDTO?> GetByIdAsync(int id);
        Task<SupportTicketDTO> CreateAsync(SupportTicket supportTicket);
        Task<bool> UpdateAsync(SupportTicket supportTicket);
        Task<bool> DeleteAsync(int id);
    }
}
