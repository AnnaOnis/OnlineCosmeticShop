using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.DB.EF.Repositories
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly AppDbContext _dbContext;
        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        protected DbSet<TEntity> Entities => _dbContext.Set<TEntity>();


        public virtual async Task<IReadOnlyList<TEntity>> GetAll(CancellationToken cancellationToken)
            => await Entities.ToListAsync(cancellationToken);

        public virtual async Task<TEntity> GetById(Guid Id, CancellationToken cancellationToken)
            => await Entities.FirstAsync(x => x.Id == Id, cancellationToken);

        public virtual async Task Add(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Entities.AddAsync(entity, cancellationToken);

        }
        public virtual Task Update(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
        public virtual async Task Delete(Guid Id, CancellationToken cancellationToken)
        {
            var entity = await Entities.FirstAsync(x => x.Id == Id, cancellationToken);
            Entities.Remove(entity);
        }

        // Универсальный метод GetAllSorted с фильтрацией, сортировкой и пагинацией
        public virtual async Task<(IReadOnlyList<TEntity>, int)> GetAllSorted(
                    CancellationToken cancellationToken,
                    Expression<Func<TEntity, bool>>? filter = null,                         // Лямбда для фильтрации
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? sorter = null,   // Делегат для сортировки
                    int pageNumber = 1,                                                     // Номер страницы
                    int pageSize = 10)                                                      // Размер страницы
        {
            IQueryable<TEntity> query = Entities;

            // Применение фильтрации
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Применение сортировки
            if (sorter != null)
            {
                query = sorter(query);
            }

            var totalEntities = await query.CountAsync(cancellationToken);

            // Применение пагинации
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var entities = await query.ToListAsync(cancellationToken);

            return (entities, totalEntities);
        }
    }
}
