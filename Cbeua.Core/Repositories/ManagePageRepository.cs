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
    public class ManagePageRepository : GenericRepository<MainPage>, IMainPageRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public ManagePageRepository(AppDbContext context, ICurrentUserService currentUser) : base(context)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public IQueryable<MainPageDTO> GetQueryableMainPageList()
        {
            var q = (from mp in _context.MainPages
                     join cmp in _context.Companies on mp.CompanyId equals cmp.CompanyId
                     where mp.CompanyId == int.Parse(_currentUser.CompanyId)
                   
                     select new MainPageDTO
                     {
                         CorouselImage1 = mp.CorouselImage1,
                         CorouselImage2 = mp.CorouselImage2,
                         CorouselImage3 = mp.CorouselImage3,
                         MainText = mp.MainText,
                         Slogan = mp.Slogan,
                         LogoImage1 = mp.LogoImage1,
                         LogoImage2 = mp.LogoImage2,
                         ContactDesc1 = mp.ContactDesc1,
                         ContactDesc2 = mp.ContactDesc2,
                         ContactLine1 = mp.ContactLine1,
                         ContactLine2 = mp.ContactLine2,
                         ContactLine3 = mp.ContactLine3,
                         Phonenum = mp.Phonenum,
                         Faxnum = mp.Faxnum,
                         Website = mp.Website,
                         Email = mp.Email,
                         RulesRegulation = mp.RulesRegulation,
                         DayQuote = mp.DayQuote,
                         CompanyId = mp.CompanyId,
                         CompanyName = cmp.ComapanyName
                     });

            return q;
        }


        public async Task<IEnumerable<MainPageDTO>> GetAllAsync()
        {
            return await GetQueryableMainPageList().ToListAsync();
        }

        public async Task<MainPageDTO> GetByIdDTOAsync(int id)
        {
            var mainPage = await GetQueryableMainPageList()
                .FirstOrDefaultAsync(m => m.MainPageId == id);
            return mainPage;
        }
    }
}
