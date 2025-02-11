using AutoMapper;
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
    [ExceptionHandlingFilter]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(CategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetCategories(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(cancellationToken);
            var categoryDtos = _mapper.Map<CategoryResponseDto[]>(categories);
            return Ok(categoryDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategoryById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);
            var responseDto = _mapper.Map<CategoryResponseDto>(category);
            return Ok(responseDto);
        }
    }
}
