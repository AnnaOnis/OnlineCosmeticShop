using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CosmeticShop.DB.EF;
using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Services;
using HttpModels;
using HttpModels.Responses;
using HttpModels.Requests;
using Mappers;
using AutoMapper;
using CosmeticShop.WebAPI.Filters;

namespace CosmeticShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExceptionHandlingFilter]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ProductService productService, IMapper mapper, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<ProductResponseDto>>> GetProducts([FromQuery] FilterDto filterDto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received filterDto: " + filterDto.Filter); // Логирование для отладки

            var (products, totalProducts) = await _productService.GetProductsAsync(cancellationToken, 
                                                                  filterDto.Filter, 
                                                                  filterDto.SortField, 
                                                                  filterDto.SortOrder ? "asc" : "desc", 
                                                                  filterDto.PageNumber, 
                                                                  filterDto.PageSize, 
                                                                  filterDto.CategoryId);

            var productDtos = _mapper.Map<ProductResponseDto[]>(products);
            var response = new PagedResponse<ProductResponseDto> { Items = productDtos, TotalItems = totalProducts };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProductById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(id, cancellationToken);
            ProductResponseDto responseDto = _mapper.Map<ProductResponseDto>(product);
            return Ok(responseDto);
        }
    }
}
