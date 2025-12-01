using Cbeua.Domain.Entities.Common;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;

namespace Cbeua.InfraCore.Repositories
{
    public class AttachmentRepository : GenericRepository<Attachment>, IAttachmentRepository
    {
        private readonly AppDbContext _context;
        public AttachmentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }

}
