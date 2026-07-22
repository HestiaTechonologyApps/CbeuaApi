using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IDirectPaymentService
    {
        Task<List<DirectPaymentDTO>> GetAllAsync();
        Task<DirectPaymentDTO?> GetByIdAsync(int id);
        Task<List<DirectPaymentDTO>> GetByMemberId(int memberid);
        Task<DirectPaymentDTO> CreateAsync(DirectPayment directPayment);
        Task<CustomApiResponse> ApproveAsync(int id, int currentUserId, bool approve);
        Task<bool> UpdateAsync(DirectPayment directPayment);
        Task<bool> DeleteAsync(int id);
    }
}
