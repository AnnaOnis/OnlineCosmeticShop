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

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CategoryRequestDto categoryRequestDto, CancellationToken cancellationToken)
        {
            string name = categoryRequestDto.CategoryName;
            var parentId = categoryRequestDto.ParentCategoryId;

            var category = await _categoryService.AddCategoryAsync(name, cancellationToken, parentId is not null ? parentId : null);
            var responseDto = _mapper.Map<CategoryResponseDto>(category);
            return Ok(responseDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategoryById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);
            var responseDto = _mapper.Map<CategoryResponseDto>(category);
            return Ok(responseDto);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] CategoryRequestDto categoryRequestDto, CancellationToken cancellationToken)
        {
            var updatingCategory = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);

            string newName = categoryRequestDto.CategoryName;
            var parrentId = categoryRequestDto.ParentCategoryId;

            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, newName, cancellationToken, parrentId);
            var responseDto = _mapper.Map<CategoryResponseDto>(updatedCategory);

            return Ok(responseDto);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _categoryService.DeleteCategoryAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
