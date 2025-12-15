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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<CategoryDTO> GetCategoryQuerableList()
        {
            var q = from c in _context.Categories
                    select new CategoryDTO
                    {
                        CategoryId = c.CategoryId,
                        Name = c.Name,
                        Abbreviation = c.Abbreviation

                        
                    };
            return q;
        }
    }
}
