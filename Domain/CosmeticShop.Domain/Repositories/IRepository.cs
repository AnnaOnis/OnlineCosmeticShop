using CosmeticShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        Task<IReadOnlyList<TEntity>> GetAllSorted(CancellationToken cancellationToken,
                                                  Expression<Func<TEntity, bool>>? filter = null,                           // Лямбда для фильтрации
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? sorter = null,    // Делегат для сортировки
                                                  int pageNumber = 1,                                                       // Номер страницы
                                                  int pageSize = 10);
        Task Add(TEntity entity, CancellationToken cancellationToken);
        Task Update(TEntity entity, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
