using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Enumerations;
using CosmeticShop.Domain.Exceptions.Order;
using CosmeticShop.Domain.Exceptions.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Services
{
    public class PaymentService
    {
        /// <summary>
        /// Service for managing payments.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGateway _paymentGateway;

        public PaymentService(IUnitOfWork unitOfWork, IPaymentGateway paymentGateway)
        {
            _unitOfWork = unitOfWork;
            _paymentGateway = paymentGateway;
        }

        public async Task<Payment> GetPaymentByOrderId(Guid orderId, CancellationToken cancellation)
        {
            var payment = await _unitOfWork.PaymentRepository.GetPaymentByOrderId(orderId, cancellation);
            if (payment == null) throw new PaymentNotFoundException();
            return payment;
        }

        /// <summary>
        /// Initializes a payment for the specified order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="cancellationToken">Token for cancelling the operation.</param>
        /// <returns>Returns the initialized payment object.</returns>
        /// <exception cref="OrderNotFoundException">Thrown if the order is not found.</exception>
        public async Task<Payment> InitializePaymentAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.OrderRepository.GetById(orderId, cancellationToken);
            if (order == null)
                throw new OrderNotFoundException("Order not found.");

            var payment = new Payment(order.Id, order.CustomerId, order.OrderPaymentMethod);

            await _unitOfWork.PaymentRepository.Add(payment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return payment;
        }

        /// <summary>
        /// Processes an online payment through the payment gateway.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="cancellationToken">Token for cancelling the operation.</param>
        /// <returns>Returns true if the payment was successful, otherwise false.</returns>
        /// <exception cref="PaymentNotFoundException">Thrown if the payment is not found.</exception>
        public async Task<Payment> ProcessOnlinePaymentAsync(Guid paymentId, CancellationToken cancellationToken)
        {
            var payment = await _unitOfWork.PaymentRepository.GetById(paymentId, cancellationToken);
            if (payment == null)
                throw new PaymentNotFoundException("Payment not found.");

            //TODO реализовать взаимодействие с внешним сервисом оплаты
            // Simulate calling the external payment gateway to process the payment.
            var isPaymentSuccessful = await _paymentGateway.ProcessPaymentAsync(payment.Order.TotalAmount);

            payment.Status = isPaymentSuccessful ? PaymentStatus.Completed : PaymentStatus.Failed;

            await _unitOfWork.PaymentRepository.Update(payment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return payment;
        }

        /// <summary>
        /// Updates the status of a payment.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="status">The new payment status.</param>
        /// <param name="cancellationToken">Token for cancelling the operation.</param>
        /// <returns>Task to update the payment status.</returns>
        /// <exception cref="PaymentNotFoundException">Thrown if the payment is not found.</exception>
        public async Task UpdatePaymentStatusAsync(Guid paymentId, PaymentStatus status, CancellationToken cancellationToken)
        {
            var payment = await _unitOfWork.PaymentRepository.FindById(paymentId, cancellationToken);
            if (payment is null)
                throw new PaymentNotFoundException("Payment not found.");

            payment.Status = status;

            await _unitOfWork.PaymentRepository.Update(payment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
