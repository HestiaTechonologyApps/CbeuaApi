using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class ExpenseMasterRepository : GenericRepository<ExpenseMaster>, IExpenseMasterRepository
    {
        private readonly AppDbContext _context;
        public ExpenseMasterRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<ExpenseMasterDTO> QueryableExpenseMasters()
        {
            var q = from em in _context.ExpenseMasters
                    join et in _context.ExpenseTypes on em.ExpenseTypeId equals et.ExpenseTypeId
                    where !em.IsDeleted
                    select new ExpenseMasterDTO
                    {
                        ExpenseMasterId = em.ExpenseMasterId,
                        ExpenseTypeId = em.ExpenseTypeId,
                        ExpenseTypeName = et.Name,
                        ExpenseDate = em.ExpenseDate,
                        Amount = em.Amount,
                        PaidTo = em.PaidTo,
                        ReferenceNo = em.ReferenceNo,
                        PaymentMode = em.PaymentMode,
                        Description = em.Description,
                        IsDeleted = em.IsDeleted
                    };
            return q;
        }

        public IQueryable<ExpenseMasterDTO> QueryableExpenseMasterById(int expenseMasterId)
        {
            var q = from em in _context.ExpenseMasters.AsNoTracking()
                    join et in _context.ExpenseTypes.AsNoTracking() on em.ExpenseTypeId equals et.ExpenseTypeId
                    where !em.IsDeleted && em.ExpenseMasterId == expenseMasterId
                    select new ExpenseMasterDTO
                    {
                        ExpenseMasterId = em.ExpenseMasterId,
                        ExpenseTypeId = em.ExpenseTypeId,
                        ExpenseTypeName = et.Name,
                        ExpenseDate = em.ExpenseDate,
                        Amount = em.Amount,
                        PaidTo = em.PaidTo,
                        ReferenceNo = em.ReferenceNo,
                        PaymentMode = em.PaymentMode,
                        Description = em.Description,
                        IsDeleted = em.IsDeleted
                    };
            return q;
        }
    }
}
