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

namespace CosmeticShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(ProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts([FromQuery] FilterDto filterDto, CancellationToken cancellationToken)
        {
            var products = await _productService.GetProductsAsync(cancellationToken, 
                                                                  filterDto.Filter, 
                                                                  filterDto.SortField, 
                                                                  filterDto.SortOrder ? "asc" : "desc", 
                                                                  filterDto.PageNumber, 
                                                                  filterDto.PageSize, 
                                                                  filterDto.CategoryId);

            var productDtos = _mapper.Map<ProductResponseDto[]>(products);
            //var productDtos = products.Select(p => new ProductResponseDto
            //{
            //    Id = p.Id,
            //    Name = p.Name,
            //    Description = p.Description,
            //    Price = p.Price,
            //    CategoryId = p.CategoryId,
            //    StockQuantity = p.StockQuantity,
            //    Rating = p.Rating,
            //    Manufacturer = p.Manufacturer,
            //    ImageUrl = p.ImageUrl
            //});

            return Ok(productDtos);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> CreateProduct(ProductRequestDto productRequestDto, CancellationToken cancellationToken)
        {
            Product newProduct = _mapper.Map<Product>(productRequestDto);

            if (newProduct == null)return BadRequest();

            await _productService.AddProductAsync(newProduct, cancellationToken);
            ProductResponseDto responseDto = _mapper.Map<ProductResponseDto>(newProduct);
            return Ok(responseDto);           
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProductById(Guid id, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(id, cancellationToken);
            ProductResponseDto responseDto = _mapper.Map<ProductResponseDto>(product);
            return Ok(responseDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponseDto>> UpdateProduct(Guid id, ProductRequestDto productRequestDto, CancellationToken cancellationToken)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
        {
            await _productService.DeleteProductAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
