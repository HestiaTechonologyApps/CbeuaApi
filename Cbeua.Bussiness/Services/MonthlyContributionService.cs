using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
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
        public string AuditTableName { get; set; } = "MONTHLYCONTRIBUTION";

        public MonthlyContributionService(IMonthlyContributionRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        // ─────────────────────────────────────────────
        // GET ALL
        // ─────────────────────────────────────────────
        public async Task<List<MonthlyContributionDTO>> GetAllAsync()
        {
            return _repo.GetQueryableMonthlyContributions().ToList();
        }

        // ─────────────────────────────────────────────
        // GET BY ID
        // ─────────────────────────────────────────────
        public async Task<MonthlyContributionDTO?> GetByIdAsync(long id)
        {
            return _repo.GetQueryableMonthlyContributions()
                        .Where(u => u.MonthlyContributionId == id)
                        .FirstOrDefault();
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

        // ─────────────────────────────────────────────
        // UPDATE
        // ─────────────────────────────────────────────
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

        // ─────────────────────────────────────────────
        // DELETE (soft-delete header only)
        // ─────────────────────────────────────────────
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

        // ─────────────────────────────────────────────
        // DELETE WITH CONTRIBUTION DATA
        // Soft-deletes header + hard-deletes master & details.
        // Blocks if ContributionStatus == "A" (approved).
        // ─────────────────────────────────────────────
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

        // ─────────────────────────────────────────────
        // UPLOAD FILE (header record only, no parsing)
        // ─────────────────────────────────────────────
        public async Task<CustomApiResponse> UploadContributionFileAsync(
            int monthCode, int yearOf,
            string fileName, string fileLocation,
            string fileType, string fileExtension,
            decimal fileSize)
        {
            var existing = _repo.GetQueryableMonthlyContributions()
                .Where(mc => mc.MonthCode == monthCode && mc.YearOf == yearOf)
                .Select(dto => _repo.GetByIdAsync(dto.MonthlyContributionId).Result)
                .FirstOrDefault();

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

        // ─────────────────────────────────────────────
        // UPLOAD AND SAVE (single-shot — mirrors old POST)
        // Saves file record, clears old data, parses file,
        // inserts master + details in one call.
        // ─────────────────────────────────────────────
        public async Task<CustomApiResponse> UploadAndSaveAsync(
            int monthCode, int yearOf,
            string fileName, string fileLocation,
            string fileType, string fileExtension,
            decimal fileSize)
        {
            try
            {
                // 1. Upsert MonthlyContribution header
                var existing = _repo.GetQueryableMonthlyContributions()
                    .Where(mc => mc.MonthCode == monthCode && mc.YearOf == yearOf)
                    .Select(dto => _repo.GetByIdAsync(dto.MonthlyContributionId).Result)
                    .FirstOrDefault();

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

                if (!System.IO.File.Exists(monthly.FileLocation))
                    return new CustomApiResponse
                    {
                        IsSucess = false,
                        Error = "File not found on disk",
                        StatusCode = 404
                    };

                // 2. Remove old ContributionMaster + details for same month/year
                var oldMasters = _repo.GetExistingContributionMasters(
                    monthCode.ToString(),
                    yearOf.ToString()
                );

                foreach (var master in oldMasters)
                {
                    var children = _repo.GetContributionDetailsByMasterId(master.ContributionMasterId);
                    _repo.RemoveContributionDetails(children);
                    _repo.RemoveContributionMaster(master);
                }
                await _repo.SaveChangesAsync();

                // 3. Parse file
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

                            if (parsedMonth == monthly.MonthCode && parsedYear == monthly.YearOf)
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
                        Error = $"No valid lines found. Errors: {string.Join(" | ", errorLines)}",
                        StatusCode = 400
                    };

                // 4. Insert ContributionMaster
                var contributionMaster = new ContributionMaster
                {
                    FileName = monthly.FileName,
                    FileLocation = monthly.FileLocation,
                    FileType = monthly.FileType,
                    FileExtension = monthly.FileExtension,
                    FileSize = monthly.FileSize,
                    Month = monthly.MonthCode.ToString(),
                    Year = monthly.YearOf.ToString(),
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

                // 5. Bulk insert details in batches of 1000
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

        // ─────────────────────────────────────────────
        // SAVE CONTRIBUTION (two-step: parse already-uploaded file)
        // ─────────────────────────────────────────────
        public async Task<CustomApiResponse> SaveContributionAsync(long monthlyContributionId)
        {
            var monthly = await _repo.GetByIdAsync(monthlyContributionId);
            if (monthly == null || monthly.IsDeleted)
                return new CustomApiResponse { IsSucess = false, Error = "Record not found", StatusCode = 404 };

            if (!System.IO.File.Exists(monthly.FileLocation))
                return new CustomApiResponse { IsSucess = false, Error = "File not found on disk", StatusCode = 404 };

            try
            {
                // Remove existing masters and details for same month/year
                var existingMasters = _repo.GetExistingContributionMasters(
                    monthly.MonthCode.ToString(),
                    monthly.YearOf.ToString()
                );

                foreach (var master in existingMasters)
                {
                    var childDetails = _repo.GetContributionDetailsByMasterId(master.ContributionMasterId);
                    _repo.RemoveContributionDetails(childDetails);
                    _repo.RemoveContributionMaster(master);
                }
                await _repo.SaveChangesAsync();

                // Parse file
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

                            if (parsedMonth == monthly.MonthCode && parsedYear == monthly.YearOf)
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

                // Insert master
                var contributionMaster = new ContributionMaster
                {
                    FileName = monthly.FileName,
                    FileLocation = monthly.FileLocation,
                    FileType = monthly.FileType,
                    FileExtension = monthly.FileExtension,
                    FileSize = monthly.FileSize,
                    Month = monthly.MonthCode.ToString(),
                    Year = monthly.YearOf.ToString(),
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

                // Bulk insert details
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

        // ─────────────────────────────────────────────
        // READ FILE (preview only, no DB writes)
        // ─────────────────────────────────────────────
        public async Task<CustomApiResponse> ReadContributionFileAsync(long monthlyContributionId)
        {
            var monthly = await _repo.GetByIdAsync(monthlyContributionId);
            if (monthly == null || monthly.IsDeleted)
                return new CustomApiResponse { IsSucess = false, Error = "Record not found", StatusCode = 404 };

            if (!System.IO.File.Exists(monthly.FileLocation))
                return new CustomApiResponse { IsSucess = false, Error = "File not found on disk", StatusCode = 404 };

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

                        if (parsedMonth == monthly.MonthCode && parsedYear == monthly.YearOf)
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
                                DpCode = line.Substring(11, 5),
                                StaffNo = line.Substring(16, 6),
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

        // ─────────────────────────────────────────────
        // PRIVATE HELPERS
        // ─────────────────────────────────────────────
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