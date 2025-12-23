using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class MainPageRepository : GenericRepository<MainPage>, IMainPageRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public MainPageRepository(AppDbContext context, ICurrentUserService currentUser) : base(context)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public IQueryable<MainPageDTO> GetQueryableMainPageList()
        {
           // int companyId = int.Parse(_currentUser.CompanyId);

            return _context.MainPages
                .AsNoTracking()
                .Include(mp => mp.Company)
               // .Where(mp => mp.CompanyId == companyId)
                .Select(mp => new MainPageDTO
                {
                    MainPageId = mp.MainPageId,
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
                    CompanyName = mp.Company.ComapanyName
                });
        }




    }
}
