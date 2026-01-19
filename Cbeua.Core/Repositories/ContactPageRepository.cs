using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using System.Linq;

namespace Cbeua.Core.Repositories
{
    public class ContactMessageRepository : GenericRepository<ContactMessage>, IContactMessageRepository
    {
        private readonly AppDbContext _context;

        public ContactMessageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<ContactMessageDTO> GetQueryableContactMessageList()
        {
            return _context.ContactMessages
                .Select(cm => new ContactMessageDTO
                {
                    ContactMessageId = cm.ContactMessageId,
                    FullName = cm.FullName,
                    PhoneNumber = cm.PhoneNumber,
                    EmailAddress = cm.EmailAddress,
                    Subject = cm.Subject,
                    Message = cm.Message,
                    SubmittedAt = cm.SubmittedAt,
                    IsRead = cm.IsRead,
                    IsReplied = cm.IsReplied,
                    AdminNotes = cm.AdminNotes,
                    RepliedAt = cm.RepliedAt,
                    IPAddress = cm.IPAddress
                })
                .OrderByDescending(cm => cm.SubmittedAt);
        }
    }
}
