using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Domain.Interfaces.IRepositories
{
    public interface IMainPageRepository : IGenericRepository<MainPage>
    {
        IQueryable<MainPageDTO> GetQueryableMainPageList();
        Task<IEnumerable<MainPageDTO>> GetAllAsync();
        Task<MainPageDTO> GetByIdDTOAsync(int id);
    }
}
