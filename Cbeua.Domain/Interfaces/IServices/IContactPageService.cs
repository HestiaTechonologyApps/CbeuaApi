using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IContactMessageService
    {
        Task<List<ContactMessageDTO>> GetAllAsync();
        Task<ContactMessageDTO?> GetByIdAsync(int id);
        Task<ContactMessageDTO> CreateAsync(ContactMessage contactMessage);
        Task<bool> UpdateAsync(ContactMessage contactMessage);
        Task<bool> DeleteAsync(int id);
        Task<ContactMessageDTO> SubmitContactFormAsync(ContactFormSubmissionDTO submission, string? ipAddress = null);
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> MarkAsRepliedAsync(int id, string? adminNotes = null);
    }
}
