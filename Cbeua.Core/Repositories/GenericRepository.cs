using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Cbeua.InfraCore.Data;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);



    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity) => _dbSet.Remove(entity);

    public void SoftDelete(T entity)
    {
        var entityType = typeof(T);

        // Check if the entity has an 'IsDeleted' property
        var isDeletedProperty = entityType.GetProperty("IsDeleted", BindingFlags.Public | BindingFlags.Instance);

        if (isDeletedProperty != null && isDeletedProperty.PropertyType == typeof(bool))
        {
            // Set IsDeleted = true
            isDeletedProperty.SetValue(entity, true);

            // Mark entity as modified
            _dbSet.Update(entity);
        }
    } 

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.Where(predicate).ToListAsync();

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.AnyAsync(predicate);

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null) =>
        predicate == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(predicate);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    public void Detach<T>(T entity) 
    {
        var entry = _context.Entry(entity);
        if (entry != null)
            entry.State = EntityState.Detached;
    }
}
