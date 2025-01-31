using AutoMapper;
using CosmeticShop.Domain.Services;
using CosmeticShop.WebAPI.Filters;
using HttpModels.Requests;
using HttpModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExceptionHandlingFilter]
    public class FavoritesController : ControllerBase
    {
        private readonly FavoriteService _favoriteService;
        private readonly IMapper _mapper;
        private readonly ILogger<FavoritesController> _logger;

        public FavoritesController(FavoriteService favoriteService, IMapper mapper, ILogger<FavoritesController> logger)
        {
            _favoriteService = favoriteService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("all/customer/{customerId}")]
        public async Task<ActionResult<ProductResponseDto[]>> GetFavoritesByCustomerId ([FromRoute] Guid customerId, CancellationToken cancellationToken)
        {
            var favorites = await _favoriteService.GetFavoritesByCustomerIdAsync(customerId, cancellationToken);
            var products = favorites.Select(favorite => _mapper.Map<ProductResponseDto>(favorite.Product)).ToArray();
            return Ok(products);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<PagedResponse<ProductResponseDto[]>>> GetFavoritesByCustomerIdPaginations ([FromRoute] Guid customerId,
            CancellationToken cancellationToken,
            [FromQuery] int page,
            [FromQuery] int pageSize)
        {
            var (favorites, totalFavorites) = await _favoriteService.GetPaginationsFavoritesByCustomerIdAsync(customerId, cancellationToken, page, pageSize);
            var products = favorites.Select(favorite => _mapper.Map<ProductResponseDto>(favorite.Product)).ToArray();
            var response = new PagedResponse<ProductResponseDto> { Items = products, TotalItems = totalFavorites };
            return Ok(response);
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<FavoriteResponseDto[]>> GetFavoritesByProductId([FromRoute] Guid productId, 
            CancellationToken cancellationToken,
            [FromQuery] int page,
            [FromQuery] int pageSize)
        {
            var favorites = await _favoriteService.GetFavoritesByProductIdAsync(productId, cancellationToken, page, pageSize);

            var response = _mapper.Map<FavoriteResponseDto[]>(favorites);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites([FromBody] FavoritesRequestDto requestDto, CancellationToken cancellationToken)
        {
            await _favoriteService.AddToFavoritesAsync(requestDto.CustomerId, requestDto.ProductId, cancellationToken);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletFromFavorites([FromQuery] Guid productId, [FromQuery] Guid customerId, CancellationToken cancellationToken)
        {
            await _favoriteService.RemoveFromFavoritesAsync(customerId, productId, cancellationToken);
            return NoContent();
        }
    }
}
