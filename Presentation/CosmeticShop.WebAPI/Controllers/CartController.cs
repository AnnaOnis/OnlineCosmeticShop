using AutoMapper;
using CosmeticShop.Domain.Services;
using HttpModels.Requests;
using HttpModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CosmeticShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        private readonly IMapper _mapper;

        public CartController(CartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CartResponseDto>> GetCart(CancellationToken cancellationToken)
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var guid = Guid.Parse(strId);

            var cart = await _cartService.GetCartByCustomerIdAsync(guid, cancellationToken);
            var cartDto = _mapper.Map<CartResponseDto>(cart);

            return Ok(cartDto);
        }

        [HttpPost]
        public async Task<ActionResult<CartResponseDto>> AddOrUpdateCartItem([FromBody] CartItemRequestDto cartItemRequestDto, CancellationToken cancellationToken)
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var guid = Guid.Parse(strId);

            await _cartService.AddItemToCartAsync(guid, cartItemRequestDto.ProductId, cartItemRequestDto.Quantity, cancellationToken);

            var cart = await _cartService.GetCartByCustomerIdAsync(guid, cancellationToken);
            var cartDto = _mapper.Map<CartResponseDto>(cart);

            return Ok(cartDto);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveCartItem(Guid productId, CancellationToken cancellationToken)
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var guid = Guid.Parse(strId);

            await _cartService.RemoveItemFromCartAsync(guid, productId, cancellationToken);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart(CancellationToken cancellationToken)
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var guid = Guid.Parse(strId);

            await _cartService.ClearCartAsync(guid, cancellationToken);

            return NoContent();
        }
    }
}
