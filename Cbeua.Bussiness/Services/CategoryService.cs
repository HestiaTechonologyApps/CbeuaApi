using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IRepositories;
using Cbeua.Domain.Interfaces.IServices;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cbeua.Bussiness.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IAuditRepository _auditRepository;
        public String AuditTableName { get; set; } = "CATEGORY";
        public CategoryService(ICategoryRepository repo, IAuditRepository auditRepository)
        {
            _repo = repo;
            _auditRepository = auditRepository;
        }

        public async Task<List<CategoryDTO>> GetAllAsync()
        {
            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            var categories = await _repo.GetAllAsync();

            foreach (var category in categories)
            {
                CategoryDTO categoryDTO = await ConvertCategoryToDTO(category);
                categoryDTOs.Add(categoryDTO);


            }

            return categoryDTOs;
        }

        public async Task<CategoryDTO?> GetByIdAsync(int id)
        {
            var q = await _repo.GetByIdAsync(id);
            if (q == null) return null;
            var categoryDTO = await ConvertCategoryToDTO(q);
            return categoryDTO;
        }

        public async Task<CategoryDTO> CreateAsync(Category category)
        {
            await _repo.AddAsync(category);
            await _repo.SaveChangesAsync();
            await this._auditRepository.LogAuditAsync<Category>(
               tableName: AuditTableName,
               action: "create",
               recordId: category.CategoryId,
               oldEntity: null,
               newEntity: category,
               changedBy: "System" // Replace with actual user info
               );
            return await ConvertCategoryToDTO(category);

        }

        private async Task<CategoryDTO> ConvertCategoryToDTO(Category category)
        {
            CategoryDTO categoryDTO = new CategoryDTO();
            categoryDTO.CategoryId = category.CategoryId;
            categoryDTO.Name = category.Name;
            categoryDTO.Abbreviation = category.Abbreviation;
           return categoryDTO;
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            var oldentity = await _repo.GetByIdAsync(category.CategoryId);
            _repo.Detach(oldentity);
            _repo.Update(category);
            await _repo.SaveChangesAsync();
            await _auditRepository.LogAuditAsync<Category>(
               tableName: AuditTableName,
               action: "update",
               recordId: category.CategoryId,
               oldEntity: oldentity,
               newEntity: category,
               changedBy: "System" // Replace with actual user info
           );
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return false;
            _repo.Delete(category);
            await _auditRepository.LogAuditAsync<Category>(
               tableName: AuditTableName,
               action: "Delete",
               recordId: category.CategoryId,
               oldEntity: category,
               newEntity: category,
               changedBy: "System" // Replace with actual user info
           );
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
