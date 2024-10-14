using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
using CosmeticShop.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.DB.EF.Repositories
{
    public class UserRepositoryEf : EfRepository<User>, IUserRepository
    {
        public UserRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<User?> FindByEmail(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException($"\"{nameof(email)}\" cannot be empty or contain only a space.", nameof(email));

            return Entities.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<IReadOnlyList<User>> GetAllSorted(CancellationToken cancellationToken,
                                                           string? filter = null,
                                                           string? sortField = null,
                                                           string? sortOrder = null,
                                                           int pageNumber = 1,
                                                           int pageSize = 10)
        {
            var users = Entities.AsQueryable();

            //фильтрация
            if (!string.IsNullOrWhiteSpace(filter))
            {
                users = users.Where(u =>
                        u.FirstName.Contains(filter) ||
                        u.LastName.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        u.Role.Equals(filter)
                        );
            }

            //сортировка
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                sortOrder = sortOrder == "asc" || sortOrder == "desc" ? sortOrder : "asc";

                users = sortField switch
                {
                    "FirstName" => sortOrder == "asc" ? users.OrderBy(o => o.FirstName) : users.OrderByDescending(o => o.FirstName),
                    "Email" => sortOrder == "asc" ? users.OrderBy(o => o.Email) : users.OrderByDescending(o => o.Email),
                    _ => sortOrder == "asc" ? users.OrderBy(o => o.LastName) : users.OrderByDescending(o => o.LastName),
                };
            }

            //пагинация
            users = users.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await users.ToListAsync(cancellationToken);
        }
    }
}
