using AutoMapper;
using CosmeticShop.Domain.Services;
using CosmeticShop.Domain.Entities;
using HttpModels;
using HttpModels.Requests;
using HttpModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using CosmeticShop.WebAPI.Filters;
using Microsoft.AspNetCore.Authorization;

namespace CosmeticShop.WebAPI.Controllers
{
    [Authorize]
    [ExceptionHandlingFilter]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly CartService _cartService;
        private readonly ReviewService _reviewService;
        private readonly PaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;
        

        public OrderController(OrderService orderService, 
                                CartService cartService,
                                ReviewService reviewService,
                                PaymentService paymentService,
                                IMapper mapper, 
                                ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _cartService = cartService;
            _reviewService = reviewService;
            _paymentService = paymentService;
            _mapper = mapper;
            _logger = logger;
        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrderById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderDetailsAsync(id, cancellationToken);
            var payment = await _paymentService.GetPaymentByOrderId(order.Id, cancellationToken);
            var responseDto = _mapper.Map<OrderResponseDto>(order);
            responseDto.OrderPaymentStatus = payment.Status;

            var reviews = await _reviewService.GetReviewsByCustomerIdAsync(order.CustomerId, cancellationToken);
            foreach (var item in responseDto.OrderItems)
            {
                var itemReviews = reviews.Where(r => r.ProductId == item.ProductId);
                item.Reviews = _mapper.Map<ReviewResponseDto[]>(itemReviews);
            }

            return Ok(responseDto);
        }

        [HttpGet("customer")]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GerCustomerOrders(CancellationToken cancellationToken)
        {
            var guid = GetCurrentCustomerId();

            var orders = await _orderService.GetCustomerOrdersAsync(guid, cancellationToken);

            var responseDtos = _mapper.Map<OrderResponseDto[]>(orders);

            return Ok(responseDtos);
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> CreateOrder([FromBody] OrderCreateRequestDto orderRequestDto, CancellationToken cancellationToken)
        {
            var currentCustomerId = GetCurrentCustomerId();

            var cart = await _cartService.GetCartByCustomerIdAsync(currentCustomerId, cancellationToken);

            List<CartItem> cartItems = new List<CartItem>();

            foreach (var orderItem in orderRequestDto.CartItems)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == orderItem.ProductId);
                if (cartItem != null)
                {
                    cartItems.Add(cartItem);
                }
                else
                {
                    return BadRequest($"Product with ID {orderItem.ProductId} not found in the cart.");
                }
            }

            var order = await _orderService.CreateOrderAsync(orderRequestDto.CustomerId,
                                                             orderRequestDto.TotalQuantity,
                                                             orderRequestDto.TotalAmount,
                                                             orderRequestDto.OrderShippingMethod,
                                                             orderRequestDto.OrderPaymentMethod,
                                                             cartItems,
                                                             cancellationToken);

            var payment = await _paymentService.InitializePaymentAsync(order.Id, cancellationToken);

            var responseDto = _mapper.Map<OrderResponseDto>(order);
            responseDto.OrderPaymentStatus = payment.Status;

            return Ok(responseDto);
        }


        [HttpPost("pay/{orderId}")]
        public async Task<ActionResult<OrderResponseDto>> OrderPaymentProcessing(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderDetailsAsync(orderId, cancellationToken);
            var responseDto = _mapper.Map<OrderResponseDto>(order);

            var payment = await _paymentService.GetPaymentByOrderId(orderId, cancellationToken);
            var processedPayment = await _paymentService.ProcessOnlinePaymentAsync(payment.Id, cancellationToken);

            responseDto.OrderPaymentStatus = processedPayment.Status;

            return Ok(responseDto);
        }

        private Guid GetCurrentCustomerId()
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (strId == null)
            {
                return Guid.Empty;
            }
            var guid = Guid.Parse(strId);
            return guid;
        }
    }
}

