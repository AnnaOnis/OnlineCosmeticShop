using CosmeticShop.Domain.Repositories;
using CosmeticShop.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.DB.EF.Repositories
{
    public class UnitOfWorkEf : IUnitOfWork
    {
        public ICartRepository CartRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IPaymentRepository PaymentRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IReviewRepository ReviewRepository { get; } 
        public IUserActionRepository UserActionRepository { get; }
        public IUserRepository UserRepository { get; }

        public IJwtTokenRepository JwtTokenRepository { get; }

        public IFavoriteRepository FavoriteRepository { get; }

        public AppDbContext dbContext;

        public UnitOfWorkEf(ICartRepository cartRepository, 
                            ICategoryRepository categoryRepository, 
                            ICustomerRepository customerRepository, 
                            IOrderRepository orderRepository, 
                            IPaymentRepository paymentRepository, 
                            IProductRepository productRepository, 
                            IReviewRepository rewiewRepository, 
                            IUserActionRepository userActionRepository, 
                            IUserRepository userRepository,
                            IJwtTokenRepository jwtTokenRepository,
                            IFavoriteRepository favoriteRepository,
                            AppDbContext appDbContext)
        {
            CartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            CategoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            CustomerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            OrderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            PaymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            ProductRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            ReviewRepository = rewiewRepository ?? throw new ArgumentNullException(nameof(rewiewRepository));
            UserActionRepository = userActionRepository ?? throw new ArgumentNullException(nameof(userActionRepository));
            UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            JwtTokenRepository = jwtTokenRepository ?? throw new ArgumentNullException(nameof(jwtTokenRepository));
            FavoriteRepository = favoriteRepository ?? throw new ArgumentNullException(nameof(favoriteRepository));
            dbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {

            return await dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
