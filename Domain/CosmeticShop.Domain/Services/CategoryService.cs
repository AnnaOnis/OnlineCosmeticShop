using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Exceptions;
using CosmeticShop.Domain.Exceptions.Category;
using CosmeticShop.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CosmeticShop.Domain.Services
{
    /// <summary>
    /// Service for managing product categories.
    /// Provides functionality to add, edit, delete, and retrieve categories.
    /// </summary>
    public class CategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CartService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="unitOfWork">An instance of <see cref="IUnitOfWork"/> to manage database operations.</param>
        /// <param name="logger">An instance of <see cref="ILogger{CartService}"/> for logging service activities.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="unitOfWork"/> or <paramref name="logger"/> is <c>null</c>.</exception>
        public CategoryService(IUnitOfWork unitOfWork, ILogger<CartService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Adds a new category to the system.
        /// </summary>
        /// <param name="name">The name of the category.</param>
        /// <returns>The created category.</returns>
        /// <exception cref="ArgumentException">Thrown when the category name is null or empty.</exception>
        /// <exception cref="InvalidCategoryNameException">Thrown when the category name is invalid or already exists.</exception>
        public async Task<Category> AddCategoryAsync(string name, CancellationToken cancellationToken, Guid? parentCategoryId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be null or empty.");
            }

            if (await _unitOfWork.CategoryRepository.ExistsByNameAsync(name, cancellationToken))
            {
                throw new InvalidCategoryNameException($"Category with name '{name}' already exists.");
            }

            var category = new Category(name, parentCategoryId);
            await _unitOfWork.CategoryRepository.Add(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return category;
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="categoryId">The ID of the category to update.</param>
        /// <param name="newName">The new name for the category.</param>
        /// <returns>The updated category.</returns>
        /// <exception cref="InvalidCategoryNameException">Thrown when the category name is invalid or already exists.</exception>
        /// <exception cref="ArgumentException">Thrown when the category name is null or empty.</exception>
        public async Task<Category> UpdateCategoryAsync(Guid categoryId, string newName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Category name cannot be null or empty.");
            }

            var category = await GetCategoryByIdAsync(categoryId, cancellationToken);

            if (await _unitOfWork.CategoryRepository.ExistsByNameAsync(newName, cancellationToken))
            {
                throw new InvalidCategoryNameException($"Category with name '{newName}' already exists.");
            }

            category.CategoryName = newName;
            await _unitOfWork.CategoryRepository.Update(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return category;
        }

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category to delete.</param>
        public async Task DeleteCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            var category = await GetCategoryByIdAsync(categoryId, cancellationToken);
            await _unitOfWork.CategoryRepository.Delete(category.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves all available categories.
        /// </summary>
        /// <returns>A list of categories.</returns>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken)
        {
            return await _unitOfWork.CategoryRepository.GetAll(cancellationToken);
        }

        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>The category.</returns>
        /// <exception cref="CategoryNotFoundException">Thrown when the category is not found.</exception>
        private async Task<Category> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.CategoryRepository.GetById(categoryId, cancellationToken);
            if (category == null)
            {
                throw new CategoryNotFoundException($"Category with ID {categoryId} not found.");
            }

            return category;
        }

    }
}
