using CosmeticShop.Domain.Repositories;
using CosmeticShop.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public IRewiewRepository RewiewRepository { get; } 
        public IUserActionRepository UserActionRepository { get; }
        public IUserRepository UserRepository { get; }

        public IJwtTokenRepository JwtTokenRepository { get; }

        private readonly AppDbContext _dbContext;

        public UnitOfWorkEf(ICartRepository cartRepository, 
                            ICategoryRepository categoryRepository, 
                            ICustomerRepository customerRepository, 
                            IOrderRepository orderRepository, 
                            IPaymentRepository paymentRepository, 
                            IProductRepository productRepository, 
                            IRewiewRepository rewiewRepository, 
                            IUserActionRepository userActionRepository, 
                            IUserRepository userRepository,
                            IJwtTokenRepository jwtTokenRepository,
                            AppDbContext appDbContext)
        {
            CartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            CategoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            CustomerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            OrderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            PaymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            ProductRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            RewiewRepository = rewiewRepository ?? throw new ArgumentNullException(nameof(rewiewRepository));
            UserActionRepository = userActionRepository ?? throw new ArgumentNullException(nameof(userActionRepository));
            UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            JwtTokenRepository = jwtTokenRepository ?? throw new ArgumentNullException(nameof(jwtTokenRepository));
            _dbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
                => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
