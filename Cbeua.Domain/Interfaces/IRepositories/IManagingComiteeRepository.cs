using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IManagingComiteeRepository : IGenericRepository<ManagingComitee>
    {
        IQueryable<ManagingComiteeDTO> GetQueryableManagingComitteeList();
        Task<IEnumerable<ManagingComiteeDTO>> GetAllAsync();
        Task<ManagingComiteeDTO> GetByIdDTOAsync(int id);


    }
}
