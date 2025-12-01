using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface ICustomerRepository:IGenericRepository<Customer>
    {
        Task<IQueryable<CustomerDTO>> GetQuerableCustomerList();
    }
}
