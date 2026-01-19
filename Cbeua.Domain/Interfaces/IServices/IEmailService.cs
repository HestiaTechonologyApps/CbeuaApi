using Cbeua.Domain.DTO;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IServices
{
    public interface IEmailService
    {
        Task<bool> SendContactFormEmailAsync(ContactFormSubmissionDTO contactForm, string recipientEmail);
    }
}