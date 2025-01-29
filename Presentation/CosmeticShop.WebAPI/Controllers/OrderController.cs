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

namespace CosmeticShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExceptionHandlingFilter]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly CartService _cartService;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;
        

        public OrderController(OrderService orderService, CartService cartService, IMapper mapper, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _mapper = mapper;
            _cartService = cartService;
            _logger = logger;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<PagedResponse<OrderResponseDto>>> GetOrders([FromQuery] FilterDto filterDto, CancellationToken cancellationToken)
        {
            var (orders, totalOrders) = await _orderService.GetAllOrdersAsync(cancellationToken, 
                                                               filterDto.Filter, 
                                                               filterDto.SortField, 
                                                               filterDto.SortOrder ? "asc" : "desc", 
                                                               filterDto.PageNumber, 
                                                               filterDto.PageSize);
            var responseDtos = _mapper.Map<OrderResponseDto[]>(orders);
            var response = new PagedResponse<OrderResponseDto> { Items = responseDtos, TotalItems = totalOrders };

            return Ok(response);
        }

        //[Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrderById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderDetailsAsync(id, cancellationToken);
            var responseDto = _mapper.Map<OrderResponseDto>(order);
            return Ok(responseDto);
        }

        //[Authorize]
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
             string orderRequestDtoJson = JsonSerializer.Serialize(orderRequestDto, new JsonSerializerOptions { WriteIndented = true });
            _logger.LogInformation("Received OrderCreateRequestDto: {@OrderCreateRequestDto}", orderRequestDtoJson);

            var currentCustomerId = GetCurrentCustomerId();

            var cart = await _cartService.GetCartByCustomerIdAsync(currentCustomerId, cancellationToken);

            List<CartItem> cartItems = new List<CartItem>();

            foreach (var orderItem in orderRequestDto.CartItems)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == orderItem.ProductId);
                if (cartItem != null)
                {
                    cartItems.Add(cartItem);
                    _logger.LogInformation(cartItem.ProductId + " - " + cartItem.Quantity);
                }
                else
                {
                    _logger.LogWarning($"Product with ID {orderItem.ProductId} not found in the cart.");
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

            var responseDto = _mapper.Map<OrderResponseDto>(order);

            return Ok(responseDto);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] OrderUpdateRequestDto orderRequestDto, CancellationToken cancellationToken)
        {
            await _orderService.UpdateOrderStatusAsync(orderRequestDto.Id, orderRequestDto.NewStatus, cancellationToken);

            return NoContent();
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _orderService.DeleteOrder(id, cancellationToken);
            return NoContent();
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

