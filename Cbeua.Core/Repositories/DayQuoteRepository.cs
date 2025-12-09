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
    public class DayQuoteRepository : GenericRepository<DayQuote>, IDayQuoteRepository
    {
        private readonly AppDbContext _context;
        public DayQuoteRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<DayQuoteDTO> GetQueryableDayQuoteList()
        {
            var q = from dq in _context.DayQuotes
                    select new DayQuoteDTO
                    {
                        DayQuoteId = dq.DayQuoteId,
                        Day = dq.Day,
                        MonthCode = dq.MonthCode,
                        ToDayQuote = dq.ToDayQuote,
                        UnformatedContent = dq.UnformatedContent
                    };
            return q;
        }
    }
}
