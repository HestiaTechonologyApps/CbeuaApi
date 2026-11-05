//using Cbeua.Domain.DTO;
//using Cbeua.Domain.Entities;
//using Cbeua.Domain.Interfaces.IRepositories;
//using Cbeua.InfraCore.Data;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Caching.Memory;
//using System.Threading;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;




//namespace Cbeua.Core.Repositories
//{
//    public class MonthlyContributionRepository : GenericRepository<MonthlyContribution>, IMonthlyContributionRepository
//    {
//        private readonly AppDbContext _context;//changed section
//        private readonly IMemoryCache _cache;

//        public MonthlyContributionRepository(AppDbContext context, IMemoryCache cache) : base(context)
//        {
//            _context = context;
//            _cache = cache;
//        }
//        //upto this

//        public IQueryable<MonthlyContributionDTO> GetQueryableMonthlyContributions()
//        {
//            var q = from mc in _context.MonthlyContributions
//                    join m in _context.Months on mc.MonthCode equals m.MonthCode
//                    join y in _context.YearMasters on mc.YearOf equals y.YearOf
//                    where !mc.IsDeleted
//                    select new MonthlyContributionDTO
//                    {
//                        MonthlyContributionId = mc.MonthlyContributionId,
//                        FileName = mc.FileName,
//                        FileLocation = mc.FileLocation,
//                        FileType = mc.FileType,
//                        FileExtension = mc.FileExtension,
//                        FileSize = mc.FileSize,
//                        MonthCode = mc.MonthCode,
//                        MonthName = m.MonthName,
//                        YearOf = mc.YearOf,
//                        YearName = y.YearName,
//                        IsDeleted = mc.IsDeleted
//                    };
//            return q;
//        }

//        public List<ContributionMaster> GetExistingContributionMasters(string month, string year)
//        {
//            return _context.ContributionMasters
//                .Where(cm => cm.Month == month && cm.Year == year)
//                .ToList();
//        }

//        public async Task AddContributionMasterAsync(ContributionMaster master)
//        {
//            await _context.ContributionMasters.AddAsync(master);
//        }

//        public async Task AddContributionDetailAsync(ContributionDetail detail)
//        {
//            await _context.ContributionDetails.AddAsync(detail);
//        }


//        public async Task AddContributionDetailsRangeAsync(List<ContributionDetail> details)
//        {
//            _context.ChangeTracker.AutoDetectChangesEnabled = false;
//            try
//            {
//                await _context.ContributionDetails.AddRangeAsync(details);
//            }
//            finally
//            {
//                _context.ChangeTracker.AutoDetectChangesEnabled = true;
//            }
//        }

//        public List<ContributionDetail> GetContributionDetailsByMasterId(long masterId)
//        {
//            return _context.ContributionDetails.AsNoTracking()
//                .Where(d => d.ContributionMasterId == masterId)
//                .ToList();
//        }
//        public async Task<List<ContributionMasterListDTO>> GetAllContributionMasters()
//        {
//            return await (
//                from cm in _context.ContributionMasters
//                join month in _context.Months
//                    on cm.Month equals month.MonthCode.ToString()
//                orderby cm.ContributionMasterId descending
//                select new ContributionMasterListDTO
//                {
//                    ContributionMasterId = cm.ContributionMasterId,
//                    FileName = cm.FileName,
//                    FileLocation = cm.FileLocation,
//                    FileType = cm.FileType,
//                    FileExtension = cm.FileExtension,
//                    FileSize = cm.FileSize,
//                    Month = cm.Month,
//                    MonthName = month.MonthName,
//                    Year = cm.Year,
//                    Circle = cm.Circle,
//                    TotalAmount = cm.totalamount,
//                    TotalEntry = cm.totalentry,
//                    ContributionStatus = cm.ContributionStatus,
//                    NewMemberCount = cm.NewMemberCount,
//                    ApprovedBy = cm.ApprovedBy,
//                    ApprovedDate = cm.ApprovedDate,
//                    IsApproved = cm.isApproved
//                }
//            ).AsNoTracking().ToListAsync();
//        }
//        public int GetContributionDetailsCountByMasterId(long masterId)
//        {
//            return _context.ContributionDetails
//                .Count(d => d.ContributionMasterId == masterId);
//        }

//        public void RemoveContributionDetails(List<ContributionDetail> details)
//        {
//            _context.ContributionDetails.RemoveRange(details);
//        }

//        public void RemoveContributionMaster(ContributionMaster master)
//        {
//            _context.ContributionMasters.Remove(master);
//        }


//        public void DetachAll()
//        {
//            var entries = _context.ChangeTracker.Entries().ToList();
//            foreach (var entry in entries)
//            {
//                entry.State = EntityState.Detached;
//            }
//        }

//        ////public async Task<int> GetNewMemberCountAsync(long contributionMasterId)
//        ////{
//        ////    return await _context.ContributionDetails
//        ////        .Where(d => d.ContributionMasterId == contributionMasterId
//        ////                 && !_context.Members.Any(m => m.StaffNo.ToString() == d.StaffNo))
//        ////        .CountAsync();
//        ////}
//        ////private async Task<HashSet<int>> GetActiveMemberStaffNosAsync()  //portion added
//        ////{
//        ////    var nos = await _context.Members
//        ////        .AsNoTracking()
//        ////        .Where(m => !m.IsDeleted)
//        ////        .Select(m => m.StaffNo)
//        ////        .ToListAsync();
//        ////    return nos.ToHashSet();
//        ////}

//        //public async Task<List<ContributionDetail>> GetNewMembersAsync(long contributionMasterId)
//        //{
//        //    var memberNos = await GetActiveMemberStaffNosAsync();

//        //    var details = await _context.ContributionDetails
//        //        .AsNoTracking()
//        //        .Where(d => d.ContributionMasterId == contributionMasterId)
//        //        .ToListAsync();

//        //    return details
//        //        .Where(d => !int.TryParse(d.StaffNo?.Trim(), out var s) || !memberNos.Contains(s))
//        //        .ToList();
//        //}

//        ////helper added
//        private async Task<HashSet<int>> GetActiveMemberStaffNosAsync()
//        {
//            var nos = await _context.Members
//                .AsNoTracking()
//                .Where(m => !m.IsDeleted)
//                .Select(m => m.StaffNo)
//                .ToListAsync();
//            return nos.ToHashSet();
//        }
//        public async Task<int> GetNewMemberCountAsync(long contributionMasterId)
//        {
//            var memberNos = await GetActiveMemberStaffNosAsync();

//            var details = await _context.ContributionDetails
//                .AsNoTracking()
//                .Where(d => d.ContributionMasterId == contributionMasterId)
//                .ToListAsync();

//            var newMemberCount = details.Count(d => !int.TryParse(d.StaffNo?.Trim(), out var n) || !memberNos.Contains(n));

//            return newMemberCount;
//        }
//        // till here
//        //reportss
//        //public async Task<List<ContributionDetail>> GetNewMembersAsync(long contributionMasterId)
//        //{
//        //    return await _context.ContributionDetails
//        //        .Where(d => d.ContributionMasterId == contributionMasterId
//        //                 && !_context.Members
//        //                     .Where(m => !m.IsDeleted)
//        //                     .Any(m => m.StaffNo.ToString() == d.StaffNo.Trim()))
//        //        .AsNoTracking()
//        //        .ToListAsync();
//        //}

//        public async Task UpdateContributionMasterAsync(ContributionMaster master)
//        {
//            _context.ContributionMasters.Update(master);
//        }
//        public async Task<ContributionMaster?> GetContributionMasterByIdAsync(long contributionMasterId)
//        {
//            return await _context.ContributionMasters
//                .FirstOrDefaultAsync(cm => cm.ContributionMasterId == contributionMasterId);
//        }
//        public IQueryable<ContributionDetail> GetContributionDetailsByMasterIdQueryable(long contributionMasterId)
//        {
//            return _context.ContributionDetails
//                .Where(d => d.ContributionMasterId == contributionMasterId);
//        }

//        public async Task<List<ContributionDetail>> GetWrongBranchAsync(long contributionMasterId)
//        {
//            return await _context.ContributionDetails
//                .Where(d => d.ContributionMasterId == contributionMasterId
//                         && !_context.Branches
//                             .Where(b => !b.IsDeleted)
//                             .Any(b => b.DpCode.ToString() == d.DpCode.Trim()))
//                .AsNoTracking()
//                .ToListAsync();
//        }

//        public async Task<List<ContributionDetail>> GetWrongCircleAsync(long contributionMasterId)
//        {
//            return await _context.ContributionDetails
//                .Where(d => d.ContributionMasterId == contributionMasterId
//                         && !_context.Circles
//                             .Where(c => !c.IsDeleted)
//                             .Any(c => c.CircleCode == d.Circle))
//                .AsNoTracking()
//                .ToListAsync();
//        }

//        public async Task<List<ContributionDetail>> GetParkedItemsAsync(long contributionMasterId)
//        {
//            return await _context.ContributionDetails
//                .Where(d => d.ContributionMasterId == contributionMasterId && d.isParked)
//                .ToListAsync();
//        }

//        public async Task<List<ContributionDetail>> GetAllDetailsAsync(long contributionMasterId)
//        {
//            return await _context.ContributionDetails
//                .Where(d => d.ContributionMasterId == contributionMasterId).AsNoTracking()
//                .ToListAsync();
//        }



//        // ✅ AFTER — single query with NOT EXISTS
//        public async Task<List<DefaulterDTO>> GetDefaultersAsync(string month, string year)
//        {
//            var paidRaw = await _context.ContributionDetails
//                .AsNoTracking()
//                .Where(d => d.Month == month && d.Year == year && !d.isParked)
//                .Select(d => d.StaffNo)
//                .Distinct()
//                .ToListAsync();

//            var paid = paidRaw
//                .Select(s => int.TryParse(s?.Trim(), out var n) ? n : (int?)null)
//                .Where(n => n.HasValue)
//                .Select(n => n!.Value)
//                .ToHashSet();

//            var members = await _context.Members
//                .AsNoTracking()
//                .Where(m => !m.IsDeleted)
//                .Select(m => new DefaulterDTO
//                {
//                    MemberId = m.MemberId,
//                    StaffNo = m.StaffNo,
//                    Name = m.Name,
//                    BranchId = m.BranchId
//                })
//                .ToListAsync();

//            return members.Where(m => !paid.Contains(m.StaffNo)).ToList();
//        }
//        //new code starts here
//        private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(2);

//        // ---------- cached lookups (small tables) ----------
//        private Task<HashSet<int>> GetActiveMemberStaffNosAsync(CancellationToken ct) =>
//            _cache.GetOrCreateAsync("lookup:staffnos", async e =>
//            {
//                e.AbsoluteExpirationRelativeToNow = Ttl;
//                var nos = await _context.Members.AsNoTracking()
//                    .Where(m => !m.IsDeleted).Select(m => m.StaffNo).ToListAsync(ct);
//                return nos.ToHashSet();
//            })!;

//        private Task<HashSet<int>> GetActiveDpCodesAsync(CancellationToken ct) =>
//            _cache.GetOrCreateAsync("lookup:dpcodes", async e =>
//            {
//                e.AbsoluteExpirationRelativeToNow = Ttl;
//                var codes = await _context.Branches.AsNoTracking()
//                    .Where(b => !b.IsDeleted).Select(b => b.DpCode).ToListAsync(ct);
//                return codes.ToHashSet();
//            })!;

//        // ---------- cached "matching IDs" per master + report ----------
//        private async Task<List<long>> GetMatchingIdsAsync(
//            long masterId, string kind, CancellationToken ct)
//        {
//            var key = $"ids:{kind}:{masterId}";
//            if (_cache.TryGetValue(key, out List<long>? cached) && cached != null)
//                return cached;

//            // only 3 narrow columns, not the whole entity
//            var rows = await _context.ContributionDetails.AsNoTracking()
//                .Where(d => d.ContributionMasterId == masterId)
//                .Select(d => new { d.ContributionDetailId, d.StaffNo, d.DpCode })
//                .ToListAsync(ct);

//            List<long> ids;
//            if (kind == "NEWMEMBERS")
//            {
//                var members = await GetActiveMemberStaffNosAsync(ct);
//                ids = rows.Where(r => !int.TryParse(r.StaffNo?.Trim(), out var s) || !members.Contains(s))
//                          .Select(r => r.ContributionDetailId).ToList();
//            }
//            else // WRONGBRANCH
//            {
//                var dps = await GetActiveDpCodesAsync(ct);
//                ids = rows.Where(r => !int.TryParse(r.DpCode?.Trim(), out var c) || !dps.Contains(c))
//                          .Select(r => r.ContributionDetailId).ToList();
//            }

//            ids.Sort();
//            _cache.Set(key, ids, Ttl);
//            return ids;
//        }

//        private async Task<(List<ContributionDetail>, int)> PageByIdsAsync(
//            List<long> ids, int page, int size, CancellationToken ct)
//        {
//            var pageIds = ids.Skip((page - 1) * size).Take(size).ToList();
//            var items = await _context.ContributionDetails.AsNoTracking()
//                .Where(d => pageIds.Contains(d.ContributionDetailId))
//                .OrderBy(d => d.ContributionDetailId)
//                .ToListAsync(ct);
//            return (items, ids.Count);
//        }

//        private static async Task<(List<ContributionDetail>, int)> PageSqlAsync(
//            IQueryable<ContributionDetail> q, int page, int size, CancellationToken ct)
//        {
//            var total = await q.CountAsync(ct);
//            var items = await q.OrderBy(d => d.ContributionDetailId)
//                               .Skip((page - 1) * size).Take(size).ToListAsync(ct);
//            return (items, total);
//        }

//        // ---------- public methods ----------
//        public async Task<(List<ContributionDetail>, int)> GetNewMembersPagedAsync(
//            long id, int page, int size, CancellationToken ct) =>
//            await PageByIdsAsync(await GetMatchingIdsAsync(id, "NEWMEMBERS", ct), page, size, ct);

//        public async Task<(List<ContributionDetail>, int)> GetWrongBranchPagedAsync(
//            long id, int page, int size, CancellationToken ct) =>
//            await PageByIdsAsync(await GetMatchingIdsAsync(id, "WRONGBRANCH", ct), page, size, ct);

//        public async Task<(List<ContributionDetail>, int)> GetWrongCirclePagedAsync(
//            long id, int page, int size, CancellationToken ct)
//        {
//            var codes = await _cache.GetOrCreateAsync("lookup:circles", async e =>
//            {
//                e.AbsoluteExpirationRelativeToNow = Ttl;
//                return await _context.Circles.AsNoTracking()
//                    .Where(c => !c.IsDeleted).Select(c => c.CircleCode).ToListAsync(ct);
//            })!;

//            return await PageSqlAsync(_context.ContributionDetails.AsNoTracking()
//                .Where(d => d.ContributionMasterId == id && !codes.Contains(d.Circle)), page, size, ct);
//        }

//        public Task<(List<ContributionDetail>, int)> GetParkedPagedAsync(
//            long id, int page, int size, CancellationToken ct) =>
//            PageSqlAsync(_context.ContributionDetails.AsNoTracking()
//                .Where(d => d.ContributionMasterId == id && d.isParked), page, size, ct);

//        public Task<(List<ContributionDetail>, int)> GetAllPagedAsync(
//            long id, int page, int size, CancellationToken ct) =>
//            PageSqlAsync(_context.ContributionDetails.AsNoTracking()
//                .Where(d => d.ContributionMasterId == id), page, size, ct);

//        public async Task<int> GetNewMemberCountAsync(long id, CancellationToken ct = default) =>
//            (await GetMatchingIdsAsync(id, "NEWMEMBERS", ct)).Count;

//        // called by other services after data changes
//        public void InvalidateReportCache(long masterId)
//        {
//            _cache.Remove($"ids:NEWMEMBERS:{masterId}");
//            _cache.Remove($"ids:WRONGBRANCH:{masterId}");
//        }

//        public void InvalidateMemberLookup() => _cache.Remove("lookup:staffnos");
//    }
//}
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.InfraCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;




namespace Cbeua.Core.Repositories
{
    public class MonthlyContributionRepository : GenericRepository<MonthlyContribution>, IMonthlyContributionRepository
    {
        private readonly AppDbContext _context;//changed section
        private readonly IMemoryCache _cache;

        public MonthlyContributionRepository(AppDbContext context, IMemoryCache cache) : base(context)
        {
            _context = context;
            _cache = cache;
        }
        //upto this

        public IQueryable<MonthlyContributionDTO> GetQueryableMonthlyContributions()
        {
            var q = from mc in _context.MonthlyContributions
                    join m in _context.Months on mc.MonthCode equals m.MonthCode
                    join y in _context.YearMasters on mc.YearOf equals y.YearOf
                    where !mc.IsDeleted
                    select new MonthlyContributionDTO
                    {
                        MonthlyContributionId = mc.MonthlyContributionId,
                        FileName = mc.FileName,
                        FileLocation = mc.FileLocation,
                        FileType = mc.FileType,
                        FileExtension = mc.FileExtension,
                        FileSize = mc.FileSize,
                        MonthCode = mc.MonthCode,
                        MonthName = m.MonthName,
                        YearOf = mc.YearOf,
                        YearName = y.YearName,
                        IsDeleted = mc.IsDeleted
                    };
            return q;
        }

        public List<ContributionMaster> GetExistingContributionMasters(string month, string year)
        {
            return _context.ContributionMasters
                .Where(cm => cm.Month == month && cm.Year == year)
                .ToList();
        }

        public async Task AddContributionMasterAsync(ContributionMaster master)
        {
            await _context.ContributionMasters.AddAsync(master);
        }

        public async Task AddContributionDetailAsync(ContributionDetail detail)
        {
            await _context.ContributionDetails.AddAsync(detail);
        }


        public async Task AddContributionDetailsRangeAsync(List<ContributionDetail> details)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                await _context.ContributionDetails.AddRangeAsync(details);
            }
            finally
            {
                _context.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }

        public List<ContributionDetail> GetContributionDetailsByMasterId(long masterId)
        {
            return _context.ContributionDetails.AsNoTracking()
                .Where(d => d.ContributionMasterId == masterId)
                .ToList();
        }
        public async Task<List<ContributionMasterListDTO>> GetAllContributionMasters()
        {
            return await (
                from cm in _context.ContributionMasters
                join month in _context.Months
                    on cm.Month equals month.MonthCode.ToString()
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
        public int GetContributionDetailsCountByMasterId(long masterId)
        {
            return _context.ContributionDetails
                .Count(d => d.ContributionMasterId == masterId);
        }

        public void RemoveContributionDetails(List<ContributionDetail> details)
        {
            _context.ContributionDetails.RemoveRange(details);
        }

        public void RemoveContributionMaster(ContributionMaster master)
        {
            _context.ContributionMasters.Remove(master);
        }


        public void DetachAll()
        {
            var entries = _context.ChangeTracker.Entries().ToList();
            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
        }

        ////public async Task<int> GetNewMemberCountAsync(long contributionMasterId)
        ////{
        ////    return await _context.ContributionDetails
        ////        .Where(d => d.ContributionMasterId == contributionMasterId
        ////                 && !_context.Members.Any(m => m.StaffNo.ToString() == d.StaffNo))
        ////        .CountAsync();
        ////}
        ////private async Task<HashSet<int>> GetActiveMemberStaffNosAsync()  //portion added
        ////{
        ////    var nos = await _context.Members
        ////        .AsNoTracking()
        ////        .Where(m => !m.IsDeleted)
        ////        .Select(m => m.StaffNo)
        ////        .ToListAsync();
        ////    return nos.ToHashSet();
        ////}

        //public async Task<List<ContributionDetail>> GetNewMembersAsync(long contributionMasterId)
        //{
        //    var memberNos = await GetActiveMemberStaffNosAsync();

        //    var details = await _context.ContributionDetails
        //        .AsNoTracking()
        //        .Where(d => d.ContributionMasterId == contributionMasterId)
        //        .ToListAsync();

        //    return details
        //        .Where(d => !int.TryParse(d.StaffNo?.Trim(), out var s) || !memberNos.Contains(s))
        //        .ToList();
        //}

        // >>> COMMENTED OUT (old slow code, kept for reference): the old private GetActiveMemberStaffNosAsync() with NO parameters,
        // >>> and the old GetNewMemberCountAsync(long contributionMasterId) with ONE parameter.
        // >>> Reason: with both old and new versions present, calls to GetNewMemberCountAsync(id) picked this old slow one.
        // >>> The fast, cached versions are further down under "new code starts here".
        ////helper added
        //private async Task<HashSet<int>> GetActiveMemberStaffNosAsync()
        //{
        //    var nos = await _context.Members
        //        .AsNoTracking()
        //        .Where(m => !m.IsDeleted)
        //        .Select(m => m.StaffNo)
        //        .ToListAsync();
        //    return nos.ToHashSet();
        //}
        //public async Task<int> GetNewMemberCountAsync(long contributionMasterId)
        //{
        //    var memberNos = await GetActiveMemberStaffNosAsync();

        //    var details = await _context.ContributionDetails
        //        .AsNoTracking()
        //        .Where(d => d.ContributionMasterId == contributionMasterId)
        //        .ToListAsync();

        //    var newMemberCount = details.Count(d => !int.TryParse(d.StaffNo?.Trim(), out var n) || !memberNos.Contains(n));

        //    return newMemberCount;
        //}
        // <<< END COMMENTED OUT
        // till here
        //reportss
        //public async Task<List<ContributionDetail>> GetNewMembersAsync(long contributionMasterId)
        //{
        //    return await _context.ContributionDetails
        //        .Where(d => d.ContributionMasterId == contributionMasterId
        //                 && !_context.Members
        //                     .Where(m => !m.IsDeleted)
        //                     .Any(m => m.StaffNo.ToString() == d.StaffNo.Trim()))
        //        .AsNoTracking()
        //        .ToListAsync();
        //}

        public async Task UpdateContributionMasterAsync(ContributionMaster master)
        {
            _context.ContributionMasters.Update(master);
        }
        public async Task<ContributionMaster?> GetContributionMasterByIdAsync(long contributionMasterId)
        {
            return await _context.ContributionMasters
                .FirstOrDefaultAsync(cm => cm.ContributionMasterId == contributionMasterId);
        }
        public IQueryable<ContributionDetail> GetContributionDetailsByMasterIdQueryable(long contributionMasterId)
        {
            return _context.ContributionDetails
                .Where(d => d.ContributionMasterId == contributionMasterId);
        }

        // >>> COMMENTED OUT (old slow code, kept for reference). Replaced by the ...PagedAsync methods further down.
        // public async Task<List<ContributionDetail>> GetWrongBranchAsync(long contributionMasterId)
        // {
        //     return await _context.ContributionDetails
        //         .Where(d => d.ContributionMasterId == contributionMasterId
        //                  && !_context.Branches
        //                      .Where(b => !b.IsDeleted)
        //                      .Any(b => b.DpCode.ToString() == d.DpCode.Trim()))
        //         .AsNoTracking()
        //         .ToListAsync();
        // }
        //
        // public async Task<List<ContributionDetail>> GetWrongCircleAsync(long contributionMasterId)
        // {
        //     return await _context.ContributionDetails
        //         .Where(d => d.ContributionMasterId == contributionMasterId
        //                  && !_context.Circles
        //                      .Where(c => !c.IsDeleted)
        //                      .Any(c => c.CircleCode == d.Circle))
        //         .AsNoTracking()
        //         .ToListAsync();
        // }
        //
        // public async Task<List<ContributionDetail>> GetParkedItemsAsync(long contributionMasterId)
        // {
        //     return await _context.ContributionDetails
        //         .Where(d => d.ContributionMasterId == contributionMasterId && d.isParked)
        //         .ToListAsync();
        // }
        //
        // public async Task<List<ContributionDetail>> GetAllDetailsAsync(long contributionMasterId)
        // {
        //     return await _context.ContributionDetails
        //         .Where(d => d.ContributionMasterId == contributionMasterId).AsNoTracking()
        //         .ToListAsync();
        // }
        // <<< END COMMENTED OUT


        // ✅ AFTER — single query with NOT EXISTS
        public async Task<List<DefaulterDTO>> GetDefaultersAsync(string month, string year)
        {
            var paidRaw = await _context.ContributionDetails
                .AsNoTracking()
                .Where(d => d.Month == month && d.Year == year && !d.isParked)
                .Select(d => d.StaffNo)
                .Distinct()
                .ToListAsync();

            var paid = paidRaw
                .Select(s => int.TryParse(s?.Trim(), out var n) ? n : (int?)null)
                .Where(n => n.HasValue)
                .Select(n => n!.Value)
                .ToHashSet();

            var members = await _context.Members
                .AsNoTracking()
                .Where(m => !m.IsDeleted)
                .Select(m => new DefaulterDTO
                {
                    MemberId = m.MemberId,
                    StaffNo = m.StaffNo,
                    Name = m.Name,
                    BranchId = m.BranchId
                })
                .ToListAsync();

            return members.Where(m => !paid.Contains(m.StaffNo)).ToList();
        }
        //new code starts here
        private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(2);

        // ---------- cached lookups (small tables) ----------
        private Task<HashSet<int>> GetActiveMemberStaffNosAsync(CancellationToken ct) =>
            _cache.GetOrCreateAsync("lookup:staffnos", async e =>
            {
                e.AbsoluteExpirationRelativeToNow = Ttl;
                var nos = await _context.Members.AsNoTracking()
                    .Where(m => !m.IsDeleted).Select(m => m.StaffNo).ToListAsync(ct);
                return nos.ToHashSet();
            })!;

        private Task<HashSet<int>> GetActiveDpCodesAsync(CancellationToken ct) =>
            _cache.GetOrCreateAsync("lookup:dpcodes", async e =>
            {
                e.AbsoluteExpirationRelativeToNow = Ttl;
                var codes = await _context.Branches.AsNoTracking()
                    .Where(b => !b.IsDeleted).Select(b => b.DpCode).ToListAsync(ct);
                return codes.ToHashSet();
            })!;

        // ---------- cached "matching IDs" per master + report ----------
        private async Task<List<long>> GetMatchingIdsAsync(
            long masterId, string kind, CancellationToken ct)
        {
            var key = $"ids:{kind}:{masterId}";
            if (_cache.TryGetValue(key, out List<long>? cached) && cached != null)
                return cached;

            // only 3 narrow columns, not the whole entity
            var rows = await _context.ContributionDetails.AsNoTracking()
                .Where(d => d.ContributionMasterId == masterId)
                .Select(d => new { d.ContributionDetailId, d.StaffNo, d.DpCode })
                .ToListAsync(ct);

            List<long> ids;
            if (kind == "NEWMEMBERS")
            {
                var members = await GetActiveMemberStaffNosAsync(ct);
                ids = rows.Where(r => !int.TryParse(r.StaffNo?.Trim(), out var s) || !members.Contains(s))
                          .Select(r => r.ContributionDetailId).ToList();
            }
            else // WRONGBRANCH
            {
                var dps = await GetActiveDpCodesAsync(ct);
                ids = rows.Where(r => !int.TryParse(r.DpCode?.Trim(), out var c) || !dps.Contains(c))
                          .Select(r => r.ContributionDetailId).ToList();
            }

            ids.Sort();
            _cache.Set(key, ids, Ttl);
            return ids;
        }

        private async Task<(List<ContributionDetail>, int)> PageByIdsAsync(
            List<long> ids, int page, int size, CancellationToken ct)
        {
            var pageIds = ids.Skip((page - 1) * size).Take(size).ToList();
            var items = await _context.ContributionDetails.AsNoTracking()
                .Where(d => pageIds.Contains(d.ContributionDetailId))
                .OrderBy(d => d.ContributionDetailId)
                .ToListAsync(ct);
            return (items, ids.Count);
        }

        private static async Task<(List<ContributionDetail>, int)> PageSqlAsync(
            IQueryable<ContributionDetail> q, int page, int size, CancellationToken ct)
        {
            var total = await q.CountAsync(ct);
            var items = await q.OrderBy(d => d.ContributionDetailId)
                               .Skip((page - 1) * size).Take(size).ToListAsync(ct);
            return (items, total);
        }

        // ---------- public methods ----------
        public async Task<(List<ContributionDetail>, int)> GetNewMembersPagedAsync(
            long id, int page, int size, CancellationToken ct) =>
            await PageByIdsAsync(await GetMatchingIdsAsync(id, "NEWMEMBERS", ct), page, size, ct);

        public async Task<(List<ContributionDetail>, int)> GetWrongBranchPagedAsync(
            long id, int page, int size, CancellationToken ct) =>
            await PageByIdsAsync(await GetMatchingIdsAsync(id, "WRONGBRANCH", ct), page, size, ct);

        public async Task<(List<ContributionDetail>, int)> GetWrongCirclePagedAsync(
            long id, int page, int size, CancellationToken ct)
        {
            var codes = await _cache.GetOrCreateAsync("lookup:circles", async e =>
            {
                e.AbsoluteExpirationRelativeToNow = Ttl;
                return await _context.Circles.AsNoTracking()
                    .Where(c => !c.IsDeleted).Select(c => c.CircleCode).ToListAsync(ct);
            })!;

            return await PageSqlAsync(_context.ContributionDetails.AsNoTracking()
                .Where(d => d.ContributionMasterId == id && !codes.Contains(d.Circle)), page, size, ct);
        }

        public Task<(List<ContributionDetail>, int)> GetParkedPagedAsync(
            long id, int page, int size, CancellationToken ct) =>
            PageSqlAsync(_context.ContributionDetails.AsNoTracking()
                .Where(d => d.ContributionMasterId == id && d.isParked), page, size, ct);

        public Task<(List<ContributionDetail>, int)> GetAllPagedAsync(
            long id, int page, int size, CancellationToken ct) =>
            PageSqlAsync(_context.ContributionDetails.AsNoTracking()
                .Where(d => d.ContributionMasterId == id), page, size, ct);

        public async Task<int> GetNewMemberCountAsync(long id, CancellationToken ct = default) =>
            (await GetMatchingIdsAsync(id, "NEWMEMBERS", ct)).Count;

        // called by other services after data changes
        public void InvalidateReportCache(long masterId)
        {
            _cache.Remove($"ids:NEWMEMBERS:{masterId}");
            _cache.Remove($"ids:WRONGBRANCH:{masterId}");
        }

        public void InvalidateMemberLookup() => _cache.Remove("lookup:staffnos");

        // >>> NEW CODE START (park / unpark totals)

        // Re-counts the TOTAL AMOUNT and TOTAL ENTRIES from rows that are NOT parked,
        // and writes them on the master record (the header numbers on the page).
        // Call this AFTER the row's parked flag has been saved.
        public async Task RecalculateMasterTotalsAsync(long masterId, CancellationToken ct = default)
        {
            var master = await _context.ContributionMasters
                .FirstOrDefaultAsync(m => m.ContributionMasterId == masterId, ct);
            if (master == null) return;                       // nothing to update

            var active = _context.ContributionDetails.AsNoTracking()
                .Where(d => d.ContributionMasterId == masterId && !d.isParked);   // only unparked rows

            var count = await active.CountAsync(ct);                             // how many rows
            var amount = await active.SumAsync(d => (long)d.Amount, ct);          // sum of their amounts

            master.totalamount = amount.ToString();          // stored as text in your table
            master.totalentry = count.ToString();
        }

        // How many rows are parked right now (for the "Parked members" field).
        // Counts how many rows are currently parked for this contribution.
        public Task<int> GetParkedCountAsync(long id, CancellationToken ct = default) =>
            _context.ContributionDetails.AsNoTracking()
                .CountAsync(d => d.ContributionMasterId == id && d.isParked, ct);

        // <<< NEW CODE END
        // >>> NEW CODE START (read-only preview, saves nothing)
        public async Task<(int Count, long Amount)> GetActiveTotalsAsync(long id, CancellationToken ct = default)
        {
            var active = _context.ContributionDetails.AsNoTracking()
                .Where(d => d.ContributionMasterId == id && !d.isParked);
            return (await active.CountAsync(ct), await active.SumAsync(d => (long)d.Amount, ct));
        }
        // <<< NEW CODE END
        // >>> NEW CODE START (read-only check: how many parked rows are also new members)
        public async Task<int> GetParkedNewMemberCountAsync(long id, CancellationToken ct = default)
        {
            var newIds = (await GetMatchingIdsAsync(id, "NEWMEMBERS", ct)).ToHashSet();
            var parkedIds = await _context.ContributionDetails.AsNoTracking()
                .Where(d => d.ContributionMasterId == id && d.isParked)
                .Select(d => d.ContributionDetailId)
                .ToListAsync(ct);
            return parkedIds.Count(x => newIds.Contains(x));
        }
        // <<< NEW CODE END
    }
}