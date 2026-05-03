using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_LearningPlatform.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var result = await dbSet.AddAsync(entity, cancellationToken);
            return result.Entity;
        }

        public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = dbSet.Where(predicate);
            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public IQueryable<TEntity> Query()
        {
            return dbSet.AsQueryable();
        }

        public void Remove(TEntity entity)
        {
            entity.MarkDeleted();
            dbSet.Update(entity);
        }

        public void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }
    }
}
