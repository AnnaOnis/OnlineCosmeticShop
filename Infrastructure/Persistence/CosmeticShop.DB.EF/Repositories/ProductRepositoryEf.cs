using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.DB.EF.Repositories
{
    public class ProductRepositoryEf : EfRepository<Product>, IProductRepository
    {
        public ProductRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
