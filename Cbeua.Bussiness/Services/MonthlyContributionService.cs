using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class MonthlyContributionService : IMonthlyContributionService
    {
        private readonly IMonthlyContributionRepository _repo;
        private readonly IAuditRepository _auditRepository;
        private readonly IYearMasterRepository _yearMasterRepository;
        public string AuditTableName { get; set; } = "MONTHLYCONTRIBUTION";

        public MonthlyContributionService(IMonthlyContributionRepository repo, IAuditRepository auditRepository, IYearMasterRepository yearMasterRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
            _yearMasterRepository = yearMasterRepository;
        }

        public async Task<List<MonthlyContributionDTO>> GetAllAsync()
        {
            return await _repo.GetQueryableMonthlyContributions().ToListAsync();
        }

        public async Task<MonthlyContributionDTO?> GetByIdAsync(long id)
        {
            return await _repo.GetQueryableMonthlyContributions()
                              .Where(u => u.MonthlyContributionId == id)
                              .FirstOrDefaultAsync();
        }

        // ─────────────────────────────────────────────
        // CREATE (bare header record)
        // ─────────────────────────────────────────────
        public async Task<MonthlyContributionDTO> CreateAsync(MonthlyContribution monthlyContribution)
        {
            monthlyContribution.IsDeleted = false;
            await _repo.AddAsync(monthlyContribution);
            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<MonthlyContribution>(
                tableName: AuditTableName,
                action: "create",
                recordId: (int)monthlyContribution.MonthlyContributionId,
                oldEntity: null,
                newEntity: monthlyContribution,
                changedBy: "System"
            );

            return await ConvertToDTO(monthlyContribution);
        }

        public async Task<bool> UpdateAsync(MonthlyContribution monthlyContribution)
        {
            var oldEntity = await _repo.GetByIdAsync(monthlyContribution.MonthlyContributionId);
            if (oldEntity == null || oldEntity.IsDeleted) return false;

            _repo.Detach(oldEntity);
            _repo.Update(monthlyContribution);
            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<MonthlyContribution>(
                tableName: AuditTableName,
                action: "update",
                recordId: (int)monthlyContribution.MonthlyContributionId,
                oldEntity: oldEntity,
                newEntity: monthlyContribution,
                changedBy: "System"
            );

            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var monthlyContribution = await _repo.GetByIdAsync(id);
            if (monthlyContribution == null || monthlyContribution.IsDeleted) return false;

            var oldEntity = CloneMonthlyContribution(monthlyContribution);
            monthlyContribution.IsDeleted = true;
            _repo.Update(monthlyContribution);

            await _auditRepository.LogAuditAsync<MonthlyContribution>(
                tableName: AuditTableName,
                action: "delete",
                recordId: (int)monthlyContribution.MonthlyContributionId,
                oldEntity: oldEntity,
                newEntity: monthlyContribution,
                changedBy: "System"
            );

            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<CustomApiResponse> DeleteWithContributionDataAsync(long monthlyContributionId)
        {
            var monthly = await _repo.GetByIdAsync(monthlyContributionId);
            if (monthly == null || monthly.IsDeleted)
                return new CustomApiResponse { IsSucess = false, Error = "Not found", StatusCode = 404 };

            var masters = _repo.GetExistingContributionMasters(
                monthly.MonthCode.ToString(),
                monthly.YearOf.ToString()
            );

            foreach (var master in masters)
            {
                if (!string.IsNullOrWhiteSpace(master.ContributionStatus)
                    && master.ContributionStatus.Trim().ToUpper() == "A")
                {
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Contribution is already approved and cannot be deleted.",
                        StatusCode = 400
                    };
                }

                var children = _repo.GetContributionDetailsByMasterId(master.ContributionMasterId);
                _repo.RemoveContributionDetails(children);
                _repo.RemoveContributionMaster(master);
            }

            var oldEntity = CloneMonthlyContribution(monthly);
            monthly.IsDeleted = true;
            monthly.ModifiedDate = DateTime.Now;
            _repo.Update(monthly);

            await _repo.SaveChangesAsync();

            await _auditRepository.LogAuditAsync<MonthlyContribution>(
                tableName: AuditTableName,
                action: "delete",
                recordId: (int)monthly.MonthlyContributionId,
                oldEntity: oldEntity,
                newEntity: monthly,
                changedBy: "System"
            );

            return new CustomApiResponse { IsSucess = true, StatusCode = 200 };
        }

        public async Task<CustomApiResponse> UploadContributionFileAsync(
            int monthCode, int yearOf,
            string fileName, string fileLocation,
            string fileType, string fileExtension,
            decimal fileSize)
        {
            var existingDto = _repo.GetQueryableMonthlyContributions()
                .Where(mc => mc.MonthCode == monthCode && mc.YearOf == yearOf)
                .FirstOrDefault();

            var existing = existingDto != null
                ? await _repo.GetByIdAsync(existingDto.MonthlyContributionId)
                : null;

            if (existing != null)
            {
                if (!string.IsNullOrEmpty(existing.FileLocation)
                    && System.IO.File.Exists(existing.FileLocation))
                {
                    try { System.IO.File.Delete(existing.FileLocation); } catch { }
                }

                existing.FileName = fileName;
                existing.FileLocation = fileLocation;
                existing.FileType = fileType;
                existing.FileExtension = fileExtension;
                existing.FileSize = fileSize;
                existing.ModifiedDate = DateTime.Now;

                _repo.Update(existing);
                await _repo.SaveChangesAsync();

                return new CustomApiResponse { IsSucess = true, Value = fileLocation, StatusCode = 200 };
            }
            else
            {
                var monthlyContribution = new MonthlyContribution
                {
                    MonthCode = monthCode,
                    YearOf = yearOf,
                    FileName = fileName,
                    FileLocation = fileLocation,
                    FileType = fileType,
                    FileExtension = fileExtension,
                    FileSize = fileSize,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false
                };

                await _repo.AddAsync(monthlyContribution);
                await _repo.SaveChangesAsync();

                return new CustomApiResponse { IsSucess = true, Value = fileLocation, StatusCode = 201 };
            }
        }

        public async Task<CustomApiResponse> UploadAndSaveAsync(
            int monthCode, int yearOf,
            string fileName, string fileLocation,
            string fileType, string fileExtension,
            decimal fileSize)
        {
            try
            {
                var existingDto = _repo.GetQueryableMonthlyContributions()
                    .Where(mc => mc.MonthCode == monthCode && mc.YearOf == yearOf)
                    .FirstOrDefault();

                var existing = existingDto != null
                    ? await _repo.GetByIdAsync(existingDto.MonthlyContributionId)
                    : null;

                MonthlyContribution monthly;

                if (existing != null)
                {
                    if (!string.IsNullOrEmpty(existing.FileLocation)
                        && existing.FileLocation != fileLocation
                        && System.IO.File.Exists(existing.FileLocation))
                    {
                        try { System.IO.File.Delete(existing.FileLocation); } catch { }
                    }

                    existing.FileName = fileName;
                    existing.FileLocation = fileLocation;
                    existing.FileType = fileType;
                    existing.FileExtension = fileExtension;
                    existing.FileSize = fileSize;
                    existing.ModifiedDate = DateTime.Now;

                    _repo.Update(existing);
                    await _repo.SaveChangesAsync();
                    monthly = existing;
                }
                else
                {
                    monthly = new MonthlyContribution
                    {
                        MonthCode = monthCode,
                        YearOf = yearOf,
                        FileName = fileName,
                        FileLocation = fileLocation,
                        FileType = fileType,
                        FileExtension = fileExtension,
                        FileSize = fileSize,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false
                    };

                    await _repo.AddAsync(monthly);
                    await _repo.SaveChangesAsync();
                }

                _repo.Detach(monthly);

                if (!System.IO.File.Exists(monthly.FileLocation))
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "File not found on disk",
                        StatusCode = 404
                    };

                int actualYear = await GetActualYear(monthly.YearOf);

                var oldMasters = _repo.GetExistingContributionMasters(
                    monthCode.ToString(),
                    actualYear.ToString()
                );

                foreach (var master in oldMasters)
                {
                    var children = _repo.GetContributionDetailsByMasterId(master.ContributionMasterId);
                    _repo.RemoveContributionDetails(children);
                    _repo.RemoveContributionMaster(master);
                }
                await _repo.SaveChangesAsync();

                var lines = System.IO.File.ReadLines(monthly.FileLocation);
                var details = new List<ContributionDetail>();
                var errorLines = new List<string>();
                int totalAmount = 0;
                int totalEntry = 0;

                foreach (var line in lines)
                {
                    try
                    {
                        if (line.Length >= 75)
                        {
                            int parsedMonth = int.Parse(line.Substring(5, 2));
                            int parsedYear = int.Parse(line.Substring(7, 4));

                            if (parsedMonth == monthly.MonthCode && parsedYear == actualYear)
                            {
                                int amount = int.Parse(line.Substring(68, 7)) / 100;
                                totalEntry++;
                                totalAmount += amount;

                                details.Add(new ContributionDetail
                                {
                                    FullString = line,
                                    Circle = int.Parse(line.Substring(0, 5)),
                                    Month = parsedMonth.ToString(),
                                    Year = parsedYear.ToString(),
                                    DpCode = int.Parse(line.Substring(11, 5)).ToString(),     
                                    StaffNo = int.Parse(line.Substring(16, 6)).ToString(),
                                    Name = line.Substring(22, 31).Trim(),
                                    Designation = line.Substring(53, 15).Trim(),
                                    Amount = amount,
                                    isParked = false,
                                    ParkReason = "",
                                    Parkedon = null,
                                    UnParkedon = null,
                                    Total = ""
                                });
                            }
                            else
                            {
                                errorLines.Add(line + " ----- Wrong Period");
                            }
                        }
                        else
                        {
                            errorLines.Add(line + " ----- Wrong Length");
                        }
                    }
                    catch (Exception lineEx)
                    {
                        errorLines.Add(line + " ----- Parse Error: " + lineEx.Message);
                    }
                }

                if (details.Count == 0)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = $"No valid lines found. Errors: {string.Join(" | ", errorLines)}",
                        StatusCode = 400
                    };

                var contributionMaster = new ContributionMaster
                {
                    FileName = monthly.FileName,
                    FileLocation = monthly.FileLocation,
                    FileType = monthly.FileType,
                    FileExtension = monthly.FileExtension,
                    FileSize = monthly.FileSize,
                    Month = monthly.MonthCode.ToString(),
                    Year = actualYear.ToString(),  // ✅ store real year
                    Circle = details[0].Circle.ToString(),
                    totalamount = totalAmount.ToString(),
                    totalentry = totalEntry.ToString(),
                    ContributionStatus = "Uploaded",
                    NewMemberCount = "0",
                    ApprovedBy = "",
                    ApprovedDate = "",
                    isApproved = false,
                    ContributionDetails = new List<ContributionDetail>()
                };

                await _repo.AddContributionMasterAsync(contributionMaster);
                await _repo.SaveChangesAsync();

                if (contributionMaster.ContributionMasterId <= 0)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "Master record was not saved (ID is 0).",
                        StatusCode = 500
                    };

                foreach (var d in details)
                    d.ContributionMasterId = contributionMaster.ContributionMasterId;

                int batchSize = 1000;
                for (int i = 0; i < details.Count; i += batchSize)
                {
                    var batch = details.Skip(i).Take(batchSize).ToList();
                    await _repo.AddContributionDetailsRangeAsync(batch);
                    await _repo.SaveChangesAsync();
                    _repo.DetachAll();
                }

                int savedCount = _repo.GetContributionDetailsCountByMasterId(
                    contributionMaster.ContributionMasterId
                );

                int newMemberCount = await _repo.GetNewMemberCountAsync(contributionMaster.ContributionMasterId);
                contributionMaster.NewMemberCount = newMemberCount.ToString();
                await _repo.UpdateContributionMasterAsync(contributionMaster);  
                await _repo.SaveChangesAsync();
               

                await _auditRepository.LogAuditAsync<ContributionMaster>(
                    tableName: "CONTRIBUTIONMASTER",
                    action: "create",
                    recordId: (int)contributionMaster.ContributionMasterId,
                    oldEntity: null,
                    newEntity: contributionMaster,
                    changedBy: "System"
                );

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = new
                    {
                        MonthlyContributionId = monthly.MonthlyContributionId,
                        ContributionMasterId = contributionMaster.ContributionMasterId,
                        TotalEntry = totalEntry,
                        TotalAmount = totalAmount,
                        SavedDetails = savedCount,
                        NewMemberCount = newMemberCount,
                        ErrorCount = errorLines.Count,
                        ErrorLines = errorLines
                    }
                };
            }
            catch (Exception ex)
            {
                return new CustomApiResponse
                {
                    IsSucess = false,
                    Error = $"Exception: {ex.Message} | Inner: {ex.InnerException?.Message}",
                    StatusCode = 500
                };
            }
        }

        public async Task<CustomApiResponse> SaveContributionAsync(long monthlyContributionId)
        {
            var monthly = await _repo.GetByIdAsync(monthlyContributionId);
            if (monthly == null || monthly.IsDeleted)
                return new CustomApiResponse { IsSucess = false, Error = "Record not found", StatusCode = 404 };

            if (!System.IO.File.Exists(monthly.FileLocation))
                return new CustomApiResponse { IsSucess = false, Error = "File not found on disk", StatusCode = 404 };

            _repo.Detach(monthly);

            int actualYear = await GetActualYear(monthly.YearOf);

            try
            {
                var existingMasters = _repo.GetExistingContributionMasters(
                    monthly.MonthCode.ToString(),
                    actualYear.ToString()
                );

                // ────────────────────────────────────────────
                // Parse the file FIRST before touching the DB
                // ────────────────────────────────────────────
                var lines = System.IO.File.ReadLines(monthly.FileLocation);
                var details = new List<ContributionDetail>();
                var errorLines = new List<string>();
                int totalAmount = 0;
                int totalEntry = 0;

                foreach (var line in lines)
                {
                    try
                    {
                        if (line.Length >= 75)
                        {
                            int parsedMonth = int.Parse(line.Substring(5, 2));
                            int parsedYear = int.Parse(line.Substring(7, 4));

                            if (parsedMonth == monthly.MonthCode && parsedYear == actualYear)
                            {
                                int amount = int.Parse(line.Substring(68, 7)) / 100;
                                totalEntry++;
                                totalAmount += amount;

                                details.Add(new ContributionDetail
                                {
                                    FullString = line,
                                    Circle = int.Parse(line.Substring(0, 5)),
                                    Month = parsedMonth.ToString(),
                                    Year = parsedYear.ToString(),
                                    DpCode = line.Substring(11, 5),
                                    StaffNo = line.Substring(16, 6),
                                    Name = line.Substring(22, 31).Trim(),
                                    Designation = line.Substring(53, 15).Trim(),
                                    Amount = amount,
                                    isParked = false,
                                    ParkReason = "",
                                    Parkedon = null,
                                    UnParkedon = null,
                                    Total = ""
                                });
                            }
                            else
                            {
                                errorLines.Add(line + " ----- Wrong Period");
                            }
                        }
                        else
                        {
                            errorLines.Add(line + " ----- Wrong Length");
                        }
                    }
                    catch (Exception lineEx)
                    {
                        errorLines.Add(line + " ----- Parse Error: " + lineEx.Message);
                    }
                }

                if (details.Count == 0)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = $"No valid lines parsed. Errors: {string.Join(" | ", errorLines)}",
                        StatusCode = 400
                    };

                // ────────────────────────────────────────────
                // Now decide: UPDATE existing master or CREATE
                // ────────────────────────────────────────────
                ContributionMaster contributionMaster;

                if (existingMasters.Any())
                {
                    // ✅ Reuse existing master — ID stays the same
                    contributionMaster = existingMasters.First();

                    // Delete only the old details
                    var children = _repo.GetContributionDetailsByMasterId(contributionMaster.ContributionMasterId);
                    _repo.RemoveContributionDetails(children);
                    await _repo.SaveChangesAsync();

                    // Update master fields in place
                    contributionMaster.FileName = monthly.FileName;
                    contributionMaster.FileLocation = monthly.FileLocation;
                    contributionMaster.FileType = monthly.FileType;
                    contributionMaster.FileExtension = monthly.FileExtension;
                    contributionMaster.FileSize = monthly.FileSize;
                    contributionMaster.Circle = details[0].Circle.ToString();
                    contributionMaster.totalamount = totalAmount.ToString();
                    contributionMaster.totalentry = totalEntry.ToString();
                    contributionMaster.ContributionStatus = "Uploaded";
                    contributionMaster.NewMemberCount = "0";
                    contributionMaster.isApproved = false;

                    await _repo.UpdateContributionMasterAsync(contributionMaster);
                    await _repo.SaveChangesAsync();
                }
                else
                {
                    // ✅ No existing master — create a fresh one
                    contributionMaster = new ContributionMaster
                    {
                        FileName = monthly.FileName,
                        FileLocation = monthly.FileLocation,
                        FileType = monthly.FileType,
                        FileExtension = monthly.FileExtension,
                        FileSize = monthly.FileSize,
                        Month = monthly.MonthCode.ToString(),
                        Year = actualYear.ToString(),
                        Circle = details[0].Circle.ToString(),
                        totalamount = totalAmount.ToString(),
                        totalentry = totalEntry.ToString(),
                        ContributionStatus = "Uploaded",
                        NewMemberCount = "0",
                        ApprovedBy = "",
                        ApprovedDate = "",
                        isApproved = false,
                        ContributionDetails = new List<ContributionDetail>()
                    };

                    await _repo.AddContributionMasterAsync(contributionMaster);
                    await _repo.SaveChangesAsync();

                    if (contributionMaster.ContributionMasterId <= 0)
                        return new CustomApiResponse
                        {
                            IsSucess = false,
                            Error = "Master record was not saved. ContributionMasterId is 0.",
                            StatusCode = 500
                        };
                }

                // ────────────────────────────────────────────
                // Save details with the stable master ID
                // ────────────────────────────────────────────
                foreach (var detail in details)
                    detail.ContributionMasterId = contributionMaster.ContributionMasterId;

                int batchSize = 1000;
                for (int i = 0; i < details.Count; i += batchSize)
                {
                    var batch = details.Skip(i).Take(batchSize).ToList();
                    await _repo.AddContributionDetailsRangeAsync(batch);
                    await _repo.SaveChangesAsync();
                    _repo.DetachAll();
                }

                int savedCount = _repo.GetContributionDetailsCountByMasterId(
                    contributionMaster.ContributionMasterId
                );

                if (savedCount == 0)
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = $"Details were not saved. Parsed {details.Count} records but 0 rows affected.",
                        StatusCode = 500
                    };

                int newMemberCount = await _repo.GetNewMemberCountAsync(contributionMaster.ContributionMasterId);
                contributionMaster.NewMemberCount = newMemberCount.ToString();
                await _repo.UpdateContributionMasterAsync(contributionMaster);
                await _repo.SaveChangesAsync();

                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = new
                    {
                        ContributionMasterId = contributionMaster.ContributionMasterId,
                        TotalEntry = totalEntry,
                        TotalAmount = totalAmount,
                        SavedDetails = savedCount,
                        NewMemberCount = newMemberCount,
                        ErrorCount = errorLines.Count,
                        ErrorLines = errorLines
                    }
                };
            }
            catch (Exception ex)
            {
                return new CustomApiResponse
                {
                    IsSucess = false,
                    Error = $"Exception: {ex.Message} | Inner: {ex.InnerException?.Message} | StackTrace: {ex.StackTrace}",
                    StatusCode = 500
                };
            }
        }

        public async Task<CustomApiResponse> ReadContributionFileAsync(long monthlyContributionId)
        {
            var monthly = await _repo.GetByIdAsync(monthlyContributionId);
            if (monthly == null || monthly.IsDeleted)
                return new CustomApiResponse { IsSucess = false, Error = "Record not found", StatusCode = 404 };

            if (!System.IO.File.Exists(monthly.FileLocation))
                return new CustomApiResponse { IsSucess = false, Error = "File not found on disk", StatusCode = 404 };

            _repo.Detach(monthly);

            int actualYear = await GetActualYear(monthly.YearOf);

            var lines = System.IO.File.ReadLines(monthly.FileLocation);
            var validLines = new List<ContributionDetailDTO>();
            var errorLines = new List<string>();
            int totalAmount = 0;
            int totalEntry = 0;

            foreach (var line in lines)
            {
                try
                {
                    if (line.Length >= 75)
                    {
                        int parsedMonth = int.Parse(line.Substring(5, 2));
                        int parsedYear = int.Parse(line.Substring(7, 4));

                        if (parsedMonth == monthly.MonthCode && parsedYear == actualYear)
                        {
                            int amount = int.Parse(line.Substring(68, 7)) / 100;
                            totalEntry++;
                            totalAmount += amount;

                            validLines.Add(new ContributionDetailDTO
                            {
                                FullString = line,
                                Circle = int.Parse(line.Substring(0, 5)),
                                Month = parsedMonth.ToString(),
                                Year = parsedYear.ToString(),
                                DpCode = int.Parse(line.Substring(11, 5)).ToString(),     
                                StaffNo = int.Parse(line.Substring(16, 6)).ToString(),
                                Name = line.Substring(22, 31).Trim(),
                                Designation = line.Substring(53, 15).Trim(),
                                Amount = amount
                            });
                        }
                        else
                        {
                            errorLines.Add(line + " ----- Wrong Period");
                        }
                    }
                    else
                    {
                        errorLines.Add(line + " ----- Wrong Length");
                    }
                }
                catch (Exception)
                {
                    errorLines.Add(line + " ----- Parse Error");
                }
            }

            return new CustomApiResponse
            {
                IsSucess = true,
                StatusCode = 200,
                Value = new ContributionParseResultDTO
                {
                    TotalEntry = totalEntry,
                    TotalAmount = totalAmount,
                    ErrorCount = errorLines.Count,
                    ValidLines = validLines,
                    ErrorLines = errorLines
                }
            };
        }

        public async Task<PagedResult<ContributionDetail>> GetPagedContributionDetailsAsync(
            long monthlyContributionId,
            ContributionDetailPaginationParams p)
        {
            var q = _repo.GetContributionDetailsQueryable(monthlyContributionId);

            if (!string.IsNullOrWhiteSpace(p.StaffNo))
                q = q.Where(d => d.StaffNo.ToLower().Contains(p.StaffNo.ToLower().Trim()));

            if (!string.IsNullOrWhiteSpace(p.Name))
                q = q.Where(d => d.Name.ToLower().Contains(p.Name.ToLower().Trim()));

            if (!string.IsNullOrWhiteSpace(p.DpCode))
                q = q.Where(d => d.DpCode.ToLower().Contains(p.DpCode.ToLower().Trim()));

            if (p.IsParked.HasValue)
                q = q.Where(d => d.isParked == p.IsParked.Value);

            if (!string.IsNullOrWhiteSpace(p.SearchTerm))
            {
                var s = p.SearchTerm.ToLower().Trim();
                q = q.Where(d =>
                    d.StaffNo.ToLower().Contains(s) ||
                    d.Name.ToLower().Contains(s) ||
                    d.DpCode.ToLower().Contains(s) ||
                    d.Designation.ToLower().Contains(s));
            }

            q = !string.IsNullOrWhiteSpace(p.SortBy)
                ? p.SortBy.ToLower() switch
                {
                    "staffno" => p.SortDescending ? q.OrderByDescending(d => d.StaffNo) : q.OrderBy(d => d.StaffNo),
                    "name" => p.SortDescending ? q.OrderByDescending(d => d.Name) : q.OrderBy(d => d.Name),
                    "dpcode" => p.SortDescending ? q.OrderByDescending(d => d.DpCode) : q.OrderBy(d => d.DpCode),
                    "amount" => p.SortDescending ? q.OrderByDescending(d => d.Amount) : q.OrderBy(d => d.Amount),
                    "designation" => p.SortDescending ? q.OrderByDescending(d => d.Designation) : q.OrderBy(d => d.Designation),
                    _ => p.SortDescending ? q.OrderByDescending(d => d.ContributionDetailId) : q.OrderBy(d => d.ContributionDetailId)
                }
                : q.OrderBy(d => d.ContributionDetailId);

            var totalRecords = await q.CountAsync();

            var pagedData = p.GetAll
                ? await q.ToListAsync()
                : await q.Skip((p.PageNumber - 1) * p.PageSize).Take(p.PageSize).ToListAsync();

            return new PagedResult<ContributionDetail>
            {
                Data = pagedData,
                TotalRecords = totalRecords,
                PageNumber = p.PageNumber,
                PageSize = p.GetAll ? totalRecords : p.PageSize
            };
        }

        public async Task<CustomApiResponse> GetAllContributionMastersAsync()
        {
            try
            {
                var masters = await _repo.GetAllContributionMasters();
                return new CustomApiResponse
                {
                    IsSucess = true,
                    StatusCode = 200,
                    Value = masters
                };
            }
            catch (Exception ex)
            {
                return new CustomApiResponse
                {
                    IsSucess = false,
                    Error = $"Exception: {ex.Message} | Inner: {ex.InnerException?.Message}",
                    StatusCode = 500
                };
            }
        }
        public async Task<CustomApiResponse> GetContributionReportAsync(
      long contributionMasterId,
      string reportType,
      int pageNumber,
      int pageSize)
        {
            try
            {
                switch (reportType.Trim().ToUpper())
                {
                    case "NEWMEMBERS":
                        var newMembers = await _repo.GetNewMembersAsync(contributionMasterId);
                        return ToPagedResponse(newMembers, pageNumber, pageSize);

                    case "WRONGBRANCH":
                        var wrongBranch = await _repo.GetWrongBranchAsync(contributionMasterId);
                        return ToPagedResponse(wrongBranch, pageNumber, pageSize);

                    case "WRONGCIRCLE":
                        var wrongCircle = await _repo.GetWrongCircleAsync(contributionMasterId);
                        return ToPagedResponse(wrongCircle, pageNumber, pageSize);

                    case "PARKEDITEMS":
                        var parked = await _repo.GetParkedItemsAsync(contributionMasterId);
                        return ToPagedResponse(parked, pageNumber, pageSize);

                    case "ALL":
                        var all = await _repo.GetAllDetailsAsync(contributionMasterId);
                        return ToPagedResponse(all, pageNumber, pageSize);

                    case "DEFAULTER":
                        var master = await _repo.GetContributionMasterByIdAsync(contributionMasterId);
                        if (master == null)
                            return new CustomApiResponse
                            {
                                IsSucess = false,
                                Error = "Contribution master not found",
                                StatusCode = 404
                            };
                        var defaulters = await _repo.GetDefaultersAsync(master.Month, master.Year);
                        return ToPagedResponse(defaulters, pageNumber, pageSize);

                    default:
                        return new CustomApiResponse
                        {
                            IsSucess = false,
                            Error = $"Unknown report type: {reportType}",
                            StatusCode = 400
                        };
                }
            }
            catch (Exception ex)
            {
                return new CustomApiResponse
                {
                    IsSucess = false,
                    Error = $"Exception: {ex.Message} | Inner: {ex.InnerException?.Message}",
                    StatusCode = 500
                };
            }
        }

        private CustomApiResponse ToPagedResponse<T>(List<T> data, int pageNumber, int pageSize)
        {
            var totalRecords = data.Count;
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            var paged = data
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new CustomApiResponse
            {
                IsSucess = true,
                StatusCode = 200,
                Value = new
                {
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Data = paged
                }
            };
        }
        private async Task<int> GetActualYear(int yearOf)
        {
            var yearMaster = await _yearMasterRepository.GetByIdAsync(yearOf);
            if (yearMaster == null || yearMaster.IsDeleted)
                throw new InvalidOperationException($"Year code {yearOf} not found.");
            return yearMaster.YearName;
        }

        private async Task<MonthlyContributionDTO> ConvertToDTO(MonthlyContribution monthlyContribution)
        {
            return new MonthlyContributionDTO
            {
                MonthlyContributionId = monthlyContribution.MonthlyContributionId,
                FileName = monthlyContribution.FileName,
                FileLocation = monthlyContribution.FileLocation,
                FileType = monthlyContribution.FileType,
                FileExtension = monthlyContribution.FileExtension,
                FileSize = monthlyContribution.FileSize,
                MonthCode = monthlyContribution.MonthCode,
                YearOf = monthlyContribution.YearOf,
                IsDeleted = monthlyContribution.IsDeleted
            };
        }

        private MonthlyContribution CloneMonthlyContribution(MonthlyContribution monthlyContribution)
        {
            return new MonthlyContribution
            {
                MonthlyContributionId = monthlyContribution.MonthlyContributionId,
                FileName = monthlyContribution.FileName,
                FileLocation = monthlyContribution.FileLocation,
                FileType = monthlyContribution.FileType,
                FileExtension = monthlyContribution.FileExtension,
                FileSize = monthlyContribution.FileSize,
                MonthCode = monthlyContribution.MonthCode,
                YearOf = monthlyContribution.YearOf,
                CreatedDate = monthlyContribution.CreatedDate,
                ModifiedDate = monthlyContribution.ModifiedDate,
                IsDeleted = monthlyContribution.IsDeleted
            };
        }
    }
}