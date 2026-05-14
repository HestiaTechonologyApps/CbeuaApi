using Cbeua.Domain.DTO;
using Cbeua.Domain.DTO.Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbeua.Core.Repositories
{
    public class ContributionMasterRepository : IContributionMasterRepository
    {
        private readonly AppDbContext _context;

        public ContributionMasterRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<ContributionMaster>> GetAllAsync()
        {
            return await _context.ContributionMasters
                .Where(m => m.ContributionStatus.Trim().ToUpper() == "FORWARD" ||
                            m.ContributionStatus.Trim().ToUpper() == "A")
                .OrderByDescending(m => m.Year)
                .ThenByDescending(m => m.Month)
                .ToListAsync();
        }
        public async Task<ContributionMaster?> GetById(long masterId)
        {
            return await _context.ContributionMasters
                .FirstOrDefaultAsync(m => m.ContributionMasterId == masterId);
        }

        public async Task<ContributionMasterDTO?> GetByIdAsync(long masterId)
        {
            return await (
                from master in _context.ContributionMasters
                join year in _context.YearMasters
                    on master.Year equals year.YearName.ToString()
                join month in _context.Months
              on master.Month equals month.MonthName.ToString()
                where master.ContributionMasterId == masterId
                select new ContributionMasterDTO
                {
                    ContributionMasterId = master.ContributionMasterId,
                    FileName = master.FileName,
                    FileLocation = master.FileLocation,
                    FileType = master.FileType,
                    FileExtension = master.FileExtension,
                    FileSize = master.FileSize,
                    Month = master.Month,
                    Year = master.Year,
                    Circle = master.Circle,
                    totalamount = master.totalamount,
                    totalentry = master.totalentry,
                    NewMemberCount = master.NewMemberCount,
                    ContributionStatus = master.ContributionStatus,
                    isApproved = master.isApproved,
                    ApprovedBy = master.ApprovedBy,
                    ApprovedDate = master.ApprovedDate,
                    YearOf = year.YearOf,
                    MonthName=month.MonthName,
                }
            ).FirstOrDefaultAsync();
        }

        public async Task AddAsync(ContributionMaster master)
        {
            await _context.ContributionMasters.AddAsync(master);
        }

        public void Update(ContributionMaster master)
        {
            _context.ContributionMasters.Update(master);
        }

        public void Delete(ContributionMaster master)
        {
            _context.ContributionMasters.Remove(master);
        }

        public async Task<List<AccountReadyDto>> GetDetailsWithLookupsAsync(long masterId)
        {
            var rawDetails = await (
                from detail in _context.ContributionDetails
                join member in _context.Members
                    on detail.StaffNo equals member.StaffNo.ToString()
                join branch in _context.Branches
                    on detail.DpCode equals branch.DpCode.ToString()
                join circle in _context.Circles
                    on detail.Circle equals circle.CircleCode
                where detail.ContributionMasterId == masterId
                      && !detail.isParked
                      && !member.IsDeleted
                      && !branch.IsDeleted
                      && !circle.IsDeleted
                select new
                {
                    detail.ContributionDetailId,
                    detail.ContributionMasterId,
                    member.MemberId,
                    branch.BranchId,
                    circle.CircleId,
                    detail.Month,
                    detail.Year,
                    detail.Amount
                }
            ).ToListAsync();

            return rawDetails.Select(d => new AccountReadyDto
            {
                ContributionDetailId = d.ContributionDetailId,
                ContributionMasterId = d.ContributionMasterId,
                MemberId = d.MemberId,
                BranchId = d.BranchId,
                CircleId = d.CircleId,
                MonthCode = int.Parse(d.Month),
                YearOf = int.Parse(d.Year),
                Amount = d.Amount
            }).ToList();
        }

        public async Task AddAccountsRangeAsync(List<Accounts> accounts)
        {
            await _context.Accounts.AddRangeAsync(accounts);
        }
       
        public async Task<bool> ContributionExistsForMonthYearAsync(string month, string year)
        {
            return await _context.ContributionMasters
                .AnyAsync(m => m.Month == month && m.Year == year);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<ParkedDetailDto>> GetParkedDetailsAsync(long masterId)
        {
            return await _context.ContributionDetails
                .Where(d => d.ContributionMasterId == masterId && d.isParked)
                .Select(d => new ParkedDetailDto
                {
                    ContributionDetailId = d.ContributionDetailId,
                    ContributionMasterId = d.ContributionMasterId,
                    FullString = d.FullString,
                    Circle = d.Circle,
                    Month = d.Month,
                    Year = d.Year,
                    DpCode = d.DpCode,
                    StaffNo = d.StaffNo,
                    Name = d.Name,
                    Designation = d.Designation,
                    Amount = d.Amount,
                    Total = d.Total,
                    ParkReason = d.ParkReason
                })
                .OrderBy(d => d.StaffNo)
                .ToListAsync();
        }
    }
}