using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Exceptions;
using CosmeticShop.Domain.Exceptions.Category;
using CosmeticShop.Domain.Exceptions.Product;
using CosmeticShop.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Services
{
    /// <summary>
    /// The product service that provides functionality to manage products.
    /// </summary>
    public class ProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CartService> _logger;

        public ProductService(IUnitOfWork unitOfWork, ILogger<CartService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Adds a new product to the catalog.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the product is null.</exception>
        public async Task AddProductAsync(Product product, CancellationToken cancellationToken)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));

            await ValidateCategoryAsync(product.CategoryId, cancellationToken);

            await _unitOfWork.ProductRepository.Add(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates an existing product in the catalog.
        /// </summary>
        /// <param name="productId">ID of the product to update.</param>
        /// <exception cref="ProductNotFoundException">Thrown when the product not found.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the product is null.</exception>
        public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));

            var existedProduct = await _unitOfWork.ProductRepository.GetById(product.Id, cancellationToken);
            if (existedProduct is null)
            {
                throw new ProductNotFoundException("Product not found");
            }
            else
            {
                await ValidateCategoryAsync(product.CategoryId, cancellationToken);
                existedProduct = product;
                await _unitOfWork.ProductRepository.Update(existedProduct, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Retrieves a list of products with optional filtering, sorting, and searching.
        /// </summary>
        /// <param name="filter">Search term for product names.</param>
        /// <param name="sortField">Field to sort by (e.g., "name", "price").</param>
        /// <param name="sortOrder">Indicates whether the sort order is ascending.</param>
        /// <param name="categoryId">Optional category ID to filter products by category.</param>
        /// <returns>A collection of products that match the criteria.</returns>
        /// <exception cref="ProductNotFoundException">Thrown when the products is null.</exception>
        public async Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken cancellationToken,
                                                                 string? filter = null,
                                                                 string sortField = "Rating", 
                                                                 string sortOrder = "asc", 
                                                                 Guid? categoryId = null)
        {
            var products = await _unitOfWork.ProductRepository.GetAll(cancellationToken);

            if (!string.IsNullOrWhiteSpace(filter))
            {
                products = products.Where(p => p.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            if (!string.IsNullOrWhiteSpace(sortField))
            {
                products = sortField.ToLower() switch
                {
                    "Name" => sortOrder == "asc" ? products.OrderBy(p => p.Name).ToList() : products.OrderByDescending(p => p.Name).ToList(),
                    "Price" => sortOrder == "asc" ? products.OrderBy(p => p.Price).ToList() : products.OrderByDescending(p => p.Price).ToList(),
                    "StockQuantity" => sortOrder == "asc" ? products.OrderBy(p => p.StockQuantity).ToList().ToList() : products.OrderByDescending(p => p.StockQuantity).ToList(),
                    "DateAdded" => sortOrder == "asc" ? products.OrderBy(p => p.DateAdded).ToList() : products.OrderByDescending(p => p.DateAdded).ToList(),
                    _ => sortOrder == "asc" ? products.OrderBy(p => p.Rating).ToList() : products.OrderByDescending(p => p.Rating).ToList()
                };
            }
            
            return products;
        }

        /// <summary>
        /// Retrieves detailed information about a specific product.
        /// </summary>
        /// <param name="productId">ID of the product to retrieve.</param>
        /// <returns>The requested product or null if not found.</returns>
        /// <exception cref="ProductNotFoundException">Thrown when the product not found.</exception>
        public async Task<Product> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.GetById(productId, cancellationToken);

            return product ?? throw new ProductNotFoundException("Product not found");
        }

        /// <summary>
        /// Deletes a product from the catalog.
        /// </summary>
        /// <param name="productId">ID of the product to delete.</param>
        /// <exception cref="ProductNotFoundException">Thrown when the product not found.</exception>
        public async Task DeleteProductAsync(Guid productId, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.GetById(productId, cancellationToken);
            if (product == null)
                throw new ProductNotFoundException("Product not found");

            await _unitOfWork.ProductRepository.Delete(productId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Validates if the given category ID exists.
        /// </summary>
        /// <param name="categoryId">The category ID to validate.</param>
        /// <exception cref="CategoryNotFoundException">Thrown when the category not found.</exception>
        private async Task ValidateCategoryAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.CategoryRepository.GetById(categoryId, cancellationToken);
            if (category == null)
                throw new CategoryNotFoundException("Category not found");
        }
    }
}
