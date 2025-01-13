using AutoMapper;
using CosmeticShop.Domain.Services;
using HttpModels.Requests;
using HttpModels.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CosmeticShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ReviewService _reviewService;
        private readonly IMapper _mapper;

        public ReviewsController(ReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetAllNotApprovedReviews(CancellationToken cancellationToken)
        {
            var reviews = await _reviewService.GetAllNotApprovedReviewsAsync(cancellationToken);
            var reviewDtos = _mapper.Map<ReviewResponseDto[]>(reviews);
            return Ok(reviewDtos);
        }

        [HttpGet("approved/{productId}")]
        public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetApprovedReviewsByProduct([FromRoute] Guid productId, CancellationToken cancellationToken)
        {
            var reviews = await _reviewService.GetApprovedReviewsByProductAsync(productId, cancellationToken);
            var reviewDtos = _mapper.Map<ReviewResponseDto[]>(reviews);
            return Ok(reviewDtos);
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetReviewsByProduct([FromRoute] Guid productId, CancellationToken cancellationToken)
        {
            var reviews = await _reviewService.GetApprovedReviewsByProductAsync(productId, cancellationToken);
            var reviewDtos = _mapper.Map<ReviewResponseDto[]>(reviews);
            return Ok(reviewDtos);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewResponseDto>> CreateReview([FromBody] ReviewCreateRequestDto reviewRequestDto, CancellationToken cancellationToken)
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var guid = Guid.Parse(strId);

            var review = await _reviewService.AddReviewAsync(
                reviewRequestDto.ProductId,
                guid,
                reviewRequestDto.Rating,
                reviewRequestDto.ReviewText,
                cancellationToken);

            var reviewDto = _mapper.Map<ReviewResponseDto>(review);
            return Ok(reviewDto);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _reviewService.DeleteReviewAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPut("{id}/approve")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveReview([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _reviewService.ApproveReviewAsync(id, cancellationToken);
            return NoContent();
        }
    }
}