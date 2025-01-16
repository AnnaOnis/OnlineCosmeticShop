using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Repositories;
using CosmeticShop.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CosmeticShop.DB.EF.Repositories
{
    public class CartRepositoryEf : EfRepository<Cart>, ICartRepository
    {
        public CartRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Cart> GetCartByCustomerId(Guid customerId, CancellationToken cancellationToken)
        {
            var cart =  await Entities
                           .Include(cart => cart.CartItems)
                           .ThenInclude(cartItem => cartItem.Product)                           
                           .FirstOrDefaultAsync(e => e.CustomerId == customerId, cancellationToken);
            if (cart == null)
            {
                throw new CartNotFoundException($"Корзина не найдена для пользователя {customerId}");
            }
            return cart;
        }

        public async Task AddOrUpdateCartItemAsync(Guid customerId, Guid productId, int quantity, decimal productPrice, CancellationToken cancellationToken)
        {
            // Загружаем корзину и связанные элементы без отслеживания
            var cart = await Entities
                .Include(c => c.CartItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);

            if (cart == null) throw new CartNotFoundException($"Cart not found for customer {customerId}.");

            // Ищем существующий CartItem
            var existingCartItem = cart.CartItems.FirstOrDefault(item => item.ProductId == productId);

            if (existingCartItem != null)
            {
                // Если товар уже в корзине, обновляем количество
                existingCartItem.Quantity += quantity;

                // Явно обновляем CartItem
                _dbContext.Entry(existingCartItem).State = EntityState.Modified;
            }
            else
            {
                // Если товара нет в корзине, добавляем новый элемент
                var cartItem = new CartItem(cart.Id, productId, quantity, productPrice);
                await _dbContext.Set<CartItem>().AddAsync(cartItem, cancellationToken);
                cart.CartItems.Add(cartItem);  // Добавляем в локальную коллекцию для пересчета
            }

            cart.TotalAmount = cart.CartItems.Sum(item => item.ProductPrice * item.Quantity);
            _dbContext.Entry(cart).Property(c => c.TotalAmount).IsModified = true;

            // Обновляем сумму корзины
            //var totalAmount = cart.CartItems.Sum(item => item.ProductPrice * item.Quantity);
            //await _dbContext.Carts
            //    .Where(c => c.Id == cart.Id)
            //    .ExecuteUpdateAsync(c => c.SetProperty(cart => cart.TotalAmount, totalAmount), cancellationToken);
        }
    }
}
