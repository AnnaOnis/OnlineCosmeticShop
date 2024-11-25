using CosmeticShop.Domain.Services;

namespace PaymentGateway
{
    public class PaymentGatewayService : IPaymentGateway
    {
        public Task<bool> ProcessPaymentAsync(decimal amount)
        {
            return Task.FromResult(true);
        }
    }
}
