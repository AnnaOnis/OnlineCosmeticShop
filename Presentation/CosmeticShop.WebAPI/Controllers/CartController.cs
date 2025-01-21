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
            var guid = GetCurrentCustomerId();

            var cart = await _cartService.GetCartByCustomerIdAsync(guid, cancellationToken);
            var cartDto = _mapper.Map<CartResponseDto>(cart);

            return Ok(cartDto);
        }

        [HttpPost("add_item")]
        public async Task<IActionResult> AddCartItem([FromBody] CartItemRequestDto cartItemRequestDto, CancellationToken cancellationToken)
        {
            var guid = GetCurrentCustomerId();

            await _cartService.AddItemToCartAsync(guid, cartItemRequestDto.ProductId, cartItemRequestDto.Quantity, cancellationToken);

            return NoContent();
        }

        [HttpPost("update_item_quantity")]
        public async Task<ActionResult<CartResponseDto>> UpdateItemQuantity([FromBody]  CartItemRequestDto cartItemRequestDto, CancellationToken cancellationToken)
        {
            var guid = GetCurrentCustomerId();

            await _cartService.UpdateCartItemQuantityAsync(guid, cartItemRequestDto.ProductId, cartItemRequestDto.Quantity, cancellationToken);

            var cart = await _cartService.GetCartByCustomerIdAsync(guid, cancellationToken);
            var cartDto = _mapper.Map<CartResponseDto>(cart);

            return Ok(cartDto);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveCartItem([FromRoute] Guid productId, CancellationToken cancellationToken)
        {
            var guid = GetCurrentCustomerId();

            await _cartService.RemoveItemFromCartAsync(guid, productId, cancellationToken);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart(CancellationToken cancellationToken)
        {
            var guid = GetCurrentCustomerId();

            await _cartService.ClearCartAsync(guid, cancellationToken);

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
