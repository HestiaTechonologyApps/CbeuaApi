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
        

        public async Task<List<ContributionMasterListDTO>> GetAllContributionMasters()
        {
            return await (
                from cm in _context.ContributionMasters
                join month in _context.Months
                    on cm.Month equals month.MonthCode.ToString()
                where cm.ContributionStatus.Trim().ToUpper() == "FORWARDED" ||
                      cm.ContributionStatus.Trim().ToUpper() == "APPROVED"
                orderby cm.ContributionMasterId descending
                select new ContributionMasterListDTO
                {
                    ContributionMasterId = cm.ContributionMasterId,
                    FileName = cm.FileName,
                    FileLocation = cm.FileLocation,
                    FileType = cm.FileType,
                    FileExtension = cm.FileExtension,
                    FileSize = cm.FileSize,
                    Month = cm.Month,
                    MonthName = month.MonthName,
                    Year = cm.Year,
                    Circle = cm.Circle,
                    TotalAmount = cm.totalamount,
                    TotalEntry = cm.totalentry,
                    ContributionStatus = cm.ContributionStatus,
                    NewMemberCount = cm.NewMemberCount,
                    ApprovedBy = cm.ApprovedBy,
                    ApprovedDate = cm.ApprovedDate,
                    IsApproved = cm.isApproved
                }
            ).AsNoTracking().ToListAsync();
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
              on master.Month equals month.MonthCode.ToString()
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
                where detail.ContributionMasterId == masterId && !detail.isParked
                select new
                {
                    detail.ContributionDetailId,
                    detail.ContributionMasterId,
                    MemberId = _context.Members
                        .Where(m => m.StaffNo.ToString() == detail.StaffNo && !m.IsDeleted)
                        .Select(m => (int?)m.MemberId)
                        .FirstOrDefault(),
                    BranchId = _context.Branches
                        .Where(b => b.DpCode.ToString() == detail.DpCode && !b.IsDeleted)
                        .Select(b => (int?)b.BranchId)
                        .FirstOrDefault(),
                    CircleId = detail.Circle == null
                        ? (int?)null
                        : _context.Circles
                            .Where(c => c.CircleCode == detail.Circle && !c.IsDeleted)
                            .Select(c => (int?)c.CircleId)
                            .FirstOrDefault(),
                    detail.Month,
                    detail.Year,
                    detail.Amount
                }
            ).ToListAsync();

            // Guard: skip rows that couldn't resolve a valid Member/Branch —
            // these would otherwise insert an Accounts row with a null/invalid FK.
            var invalidRows = rawDetails.Where(d => d.MemberId == null || d.BranchId == null).ToList();
            if (invalidRows.Any())
            {
                // TODO: decide how you want to surface these — log, park, or throw.
                // For now they're filtered out silently below.
            }

            return rawDetails
                .Where(d => d.MemberId != null && d.BranchId != null)
                .Select(d => new AccountReadyDto
                {
                    ContributionDetailId = d.ContributionDetailId,
                    ContributionMasterId = d.ContributionMasterId,
                    MemberId = d.MemberId.Value,
                    BranchId = d.BranchId.Value,
                    CircleId = d.CircleId ?? 0, // adjust default/handling if CircleId is required downstream
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