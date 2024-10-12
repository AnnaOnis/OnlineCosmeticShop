using CosmeticShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Repositories
{
    /// <summary>
    /// Generic repository interface for CRUD operations.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> GetById(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<TEntity>> GetAll(CancellationToken cancellationToken);
        Task Add(TEntity entity, CancellationToken cancellationToken);
        Task Update(TEntity entity, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
