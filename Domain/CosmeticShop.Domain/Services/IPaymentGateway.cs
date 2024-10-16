using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Services
{
    public interface IPaymentGateway
    {
        Task<bool> ProcessPaymentAsync(decimal amount);
    }
}
