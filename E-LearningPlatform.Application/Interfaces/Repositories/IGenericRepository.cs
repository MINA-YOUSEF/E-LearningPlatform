using E_LearningPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        IQueryable<TEntity> Query();
        Task<IReadOnlyCollection<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate, int? skip = null, int? take = null,
            CancellationToken cancellationToken = default);
        Task<TEntity?> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
