using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;

namespace Cbeua.CORE.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public CustomerRepository(AppDbContext context, ICurrentUserService currentUser) : base(context)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<IQueryable<CustomerDTO>> GetQuerableCustomerList()
        {
            var q = (from cust in _context.Customers
                     join cmp in _context.Companies on cust.CompanyId equals cmp.CompanyId
                     where cust.CompanyId == int.Parse(_currentUser.CompanyId)
                     where cust.IsDeleted == false
                     select new CustomerDTO
                     {
                         CustomerId = cust.CustomerId,
                         CustomerName = cust.CustomerName,
                         IsActive = cust.IsActive,
                         CustomerPhone = cust.CustomerPhone,
                         CustomerEmail = cust.CustomerEmail,
                         ComapanyName = cmp.ComapanyName,
                         CompanyId = cust.CompanyId,
                         IsDeleted = cust.IsDeleted,
                         CustomerAddress = cust.CustomerAddress,
                         DOB = cust.DOB,
                         DOBString = cust.DOB.ToString("dd MMMM yyyy hh:mm tt"),
                         Nationality = cust.Nationalilty,
                     }).AsQueryable();
            return q;
        }
    }
}
