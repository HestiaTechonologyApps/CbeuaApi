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
    public class ExpenseTypeRepository : GenericRepository<ExpenseType>, IExpenseTypeRepository
    {
        private readonly AppDbContext _context;
        public ExpenseTypeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<ExpenseTypeDTO> QueryableExpenseTypes()
        {
            var q = from et in _context.ExpenseTypes
                    where !et.IsDeleted
                    select new ExpenseTypeDTO
                    {
                        ExpenseTypeId = et.ExpenseTypeId,
                        Name = et.Name,
                        Description = et.Description,
                        IsDeleted = et.IsDeleted
                    };
            return q;
        }
    }
}
