using AutoMapper;
using CosmeticShop.Domain.Services;
using HttpModels;
using HttpModels.Requests;
using HttpModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CosmeticShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(OrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
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
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var guid = Guid.Parse(strId);

            var orders = await _orderService.GetCustomerOrdersAsync(guid, cancellationToken);

            var responseDtos = _mapper.Map<OrderResponseDto[]>(orders);

            return Ok(responseDtos);
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> CreateOrder([FromBody] OrderCreateRequestDto orderRequestDto, CancellationToken cancellationToken)
        {
            var order = await _orderService.CreateOrderAsync(orderRequestDto.CustomerId,
                                                             orderRequestDto.OrderShippingMethod,
                                                             orderRequestDto.OrderPaymentMethod,
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
    }
}

