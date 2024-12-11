using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Exceptions.Product;
using CosmeticShop.Domain.Exceptions.Review;
using CosmeticShop.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace CosmeticShop.Domain.Services
{
    /// <summary>
    /// Service for managing reviews. Includes functionality for adding, viewing, and deleting reviews.
    /// </summary>
    public class ReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;

        public ReviewService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Adds a new review to the product.
        /// </summary>
        /// <param name="productId">ID of the product to review</param>
        /// <param name="customerId">ID of the customer leaving the review</param>
        /// <param name="rating">Rating given by the customer</param>
        /// <param name="comment">Comment left by the customer</param>
        /// <returns>Asynchronous operation</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the rating is out of the 1.0 to 5.0 range.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the review text is null or contains only a space.</exception>
        /// <exception cref="ProductNotFoundException">Throw when product not found.</exception>
        public async Task<Review> AddReviewAsync(Guid productId, 
                                         Guid customerId, 
                                         int rating, 
                                         string comment,
                                         CancellationToken cancellationToken)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException("Rating must be between 1 and 5.", nameof(rating));

            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentNullException("Review text cannot be null or contains only a space.", nameof(comment));

            var product = await _unitOfWork.ProductRepository.GetById(productId, cancellationToken);
            if (product == null)
            {
                throw new ProductNotFoundException("Product not found.");
            }

            var review = new Review(productId, customerId, rating, comment);

            await _unitOfWork.ReviewRepository.Add(review, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return review;
        }

        /// <summary>
        /// Returns all reviews for a specific product.
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <returns>List of reviews for the product</returns>
        /// <exception cref="ProductNotFoundException">Throw when product not found.</exception>
        public async Task<IReadOnlyList<Review>> GetReviewsByProductIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.GetById(productId, cancellationToken);
            if (product == null)
            {
                throw new ProductNotFoundException("Product not found.");
            }

            return await _unitOfWork.ReviewRepository.GetByProductId(productId, cancellationToken);
        }

        /// <summary>
        /// Returns all reviews for a specific product.
        /// </summary>
        /// <param name="productId">ID of the product</param>
        /// <returns>List of reviews for the product</returns>
        /// <exception cref="ProductNotFoundException">Throw when product not found.</exception>
        public async Task<IReadOnlyList<Review>> GetApprovedReviewsByProductAsync(Guid productId, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.GetById(productId, cancellationToken);
            if (product == null)
            {
                throw new ProductNotFoundException("Product not found.");
            }

            return await _unitOfWork.ReviewRepository.GetApprovedReviewsByProductId(productId, cancellationToken);
        }

        /// <summary>
        /// Deletes a review by its ID. Only administrators can perform this action.
        /// </summary>
        /// <param name="reviewId">ID of the review</param>
        /// <returns>Asynchronous operation</returns>
        /// <exception cref="ReviewNotFoundException">Throw when review not found.</exception>
        public async Task DeleteReviewAsync(Guid reviewId, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.ReviewRepository.GetById(reviewId, cancellationToken);
            if (review == null)
            {
                throw new ReviewNotFoundException("Review not found.");
            }

            await _unitOfWork.ReviewRepository.Delete(review.Id , cancellationToken);
        }

        /// <summary>
        /// Approves the review and updates the product's rating based on approved reviews.
        /// </summary>
        /// <param name="reviewId">ID of the review to approve</param>
        /// <returns>Asynchronous operation</returns>
        /// <exception cref="ReviewNotFoundException">Throw when review not found.</exception>
        /// <exception cref="ReviewAlreadyApprovedException">Throw when review is already approved.</exception>
        /// <exception cref="ProductNotFoundException">Throw when product not found.</exception>
        public async Task ApproveReviewAsync(Guid reviewId, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.ReviewRepository.GetById(reviewId, cancellationToken);
            if (review == null)
            {
                throw new ReviewNotFoundException("Review not found.");
            }

            if (review.IsApproved)
            {
                throw new ReviewAlreadyApprovedException("Review is already approved.");
            }

            // Одобряем отзыв
            review.IsApproved = true;
            await _unitOfWork.ReviewRepository.Update(review, cancellationToken);

            // Обновляем рейтинг товара
            var product = await _unitOfWork.ProductRepository.GetById(review.ProductId, cancellationToken);
            if (product == null)
            {
                throw new ProductNotFoundException("Product not found.");
            }

            // Получаем все одобренные отзывы для данного товара
            var approvedReviews = await _unitOfWork.ReviewRepository.GetApprovedReviewsByProductId(review.ProductId, cancellationToken);

            // Рассчитываем средний рейтинг
            product.Rating = approvedReviews.Average(r => r.Rating);

            // Обновляем информацию о товаре
            await _unitOfWork.ProductRepository.Update(product, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously retrieves a read-only list of reviews that have not yet been approved.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation if needed.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a read-only list of unapproved reviews.</returns>
        /// <remarks>
        /// This method interacts with the repository layer to fetch all reviews where the `IsApproved` 
        /// property is set to `false`. It returns the result as an asynchronous task.
        /// </remarks>
        public async Task<IReadOnlyList<Review>> GetAllNotApprovedReviewsAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.ReviewRepository.GetAllNotApprovedReviews(cancellationToken);
        }
    }
}
