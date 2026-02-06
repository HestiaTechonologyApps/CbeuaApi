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

        public async Task<List<MonthlyContributionDTO>> GetAllAsync()
        {
            return _repo.GetQueryableMonthlyContributions().ToList();
        }

        public async Task<MonthlyContributionDTO?> GetByIdAsync(long id)
        {
            var q = _repo.GetQueryableMonthlyContributions();
            var contribution = q.Where(u => u.MonthlyContributionId == id).FirstOrDefault();
            return contribution;
        }

        public async Task<MonthlyContributionDTO> CreateAsync(MonthlyContribution monthlyContribution)
        {
            monthlyContribution.IsDeleted = false; // ✅ ENSURE NOT DELETED
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

        private async Task<MonthlyContributionDTO> ConvertToDTO(MonthlyContribution monthlyContribution)
        {
            MonthlyContributionDTO dto = new MonthlyContributionDTO();
            dto.MonthlyContributionId = monthlyContribution.MonthlyContributionId;
            dto.FileName = monthlyContribution.FileName;
            dto.FileLocation = monthlyContribution.FileLocation;
            dto.FileType = monthlyContribution.FileType;
            dto.FileExtension = monthlyContribution.FileExtension;
            dto.FileSize = monthlyContribution.FileSize;
            dto.MonthCode = monthlyContribution.MonthCode;
            dto.YearOf = monthlyContribution.YearOf;
            dto.IsDeleted = monthlyContribution.IsDeleted; // ✅ ADDED
            return dto;
        }

        // ✅ ADDED CLONE METHOD
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

        public async Task<bool> UpdateAsync(MonthlyContribution monthlyContribution)
        {
            var oldEntity = await _repo.GetByIdAsync(monthlyContribution.MonthlyContributionId);
            if (oldEntity == null || oldEntity.IsDeleted) return false; // ✅ CHECK IF DELETED

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
            if (monthlyContribution == null || monthlyContribution.IsDeleted) return false; // ✅ CHECK IF ALREADY DELETED

            var oldEntity = CloneMonthlyContribution(monthlyContribution); // ✅ CLONE FOR AUDIT

            // ✅ SOFT DELETE
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

        public async Task<CustomApiResponse> UploadContributionFileAsync(int monthCode, int yearOf, string fileName, string fileLocation, string fileType, string fileExtension, decimal fileSize)
        {
            // Check if contribution already exists for this month/year (excluding deleted ones)
            var existing = _repo.GetQueryableMonthlyContributions()
                .Where(mc => mc.MonthCode == monthCode && mc.YearOf == yearOf)
                .Select(dto => _repo.GetByIdAsync(dto.MonthlyContributionId).Result)
                .FirstOrDefault();

            if (existing != null)
            {
                // Delete old file if exists
                if (!string.IsNullOrEmpty(existing.FileLocation) && System.IO.File.Exists(existing.FileLocation))
                {
                    try { System.IO.File.Delete(existing.FileLocation); } catch { }
                }

                // Update existing record
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
                // Create new record
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
                    IsDeleted = false // ✅ ENSURE NOT DELETED
                };

                await _repo.AddAsync(monthlyContribution);
                await _repo.SaveChangesAsync();

                return new CustomApiResponse { IsSucess = true, Value = fileLocation, StatusCode = 201 };
            }
        }
    }
}