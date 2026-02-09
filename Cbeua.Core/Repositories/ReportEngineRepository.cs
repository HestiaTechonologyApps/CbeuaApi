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
    public class ReportEngineRepository : GenericRepository<ReportEngine>, IReportEngineRepository
    {
        private readonly AppDbContext _context;

        public ReportEngineRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

    }
}