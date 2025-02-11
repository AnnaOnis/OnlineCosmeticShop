using AutoMapper;
using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Services;
using CosmeticShop.WebAPI.Filters;
using HttpModels.Requests;
using HttpModels.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [ExceptionHandlingFilter]
    public class AdminController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly CustomerService _customerService;
        private readonly OrderService _orderService;
        private readonly ReviewService _reviewService;
        private readonly ProductService _productService;
        private readonly UserService _userService;
        private readonly PaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;

        public AdminController(CategoryService categoryService,
                               CustomerService customerService,
                               OrderService orderService,
                               ReviewService reviewService,
                               ProductService productService,
                               UserService userService,
                               PaymentService paymentService,
                               IMapper mapper,
                               ILogger<AdminController> logger)
        {
            _categoryService = categoryService;
            _customerService = customerService;
            _orderService = orderService;
            _reviewService = reviewService;
            _productService = productService;
            _userService = userService;
            _paymentService = paymentService;
            _mapper = mapper;
            _logger = logger;
        }

        //Работа с товарами

        [HttpPost("product/create")]
        public async Task<ActionResult<ProductResponseDto>> CreateProduct([FromBody] ProductRequestDto productRequestDto, CancellationToken cancellationToken)
        {
            Product newProduct = _mapper.Map<Product>(productRequestDto);

            if (newProduct == null) return BadRequest();

            await _productService.AddProductAsync(newProduct, cancellationToken);
            ProductResponseDto responseDto = _mapper.Map<ProductResponseDto>(newProduct);
            return Ok(responseDto);
        }

        [HttpPut("product/{id}")]
        public async Task<ActionResult<ProductResponseDto>> UpdateProduct([FromRoute] Guid id, [FromBody] ProductRequestDto productRequestDto, CancellationToken cancellationToken)
        {
            var updatingProduct = await _productService.GetProductByIdAsync(id, cancellationToken);

            updatingProduct.Name = productRequestDto.Name;
            updatingProduct.Price = productRequestDto.Price;
            updatingProduct.Description = productRequestDto.Description;
            updatingProduct.Manufacturer = productRequestDto.Manufacturer;
            updatingProduct.CategoryId = productRequestDto.CategoryId;
            updatingProduct.Rating = productRequestDto.Rating;
            updatingProduct.StockQuantity = productRequestDto.StockQuantity;
            updatingProduct.ImageUrl = productRequestDto.ImageUrl;

            await _productService.UpdateProductAsync(updatingProduct, cancellationToken);

            ProductResponseDto responseDto = _mapper.Map<ProductResponseDto>(updatingProduct);
            return Ok(responseDto);
        }

        [HttpDelete("product/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _productService.DeleteProductAsync(id, cancellationToken);
            return NoContent();
        }

        //Работа с заказами и платежами


    }
}
