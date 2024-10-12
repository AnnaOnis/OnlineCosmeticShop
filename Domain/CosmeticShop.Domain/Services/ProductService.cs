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
        public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
        {
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
        /// <param name="search">Search term for product names.</param>
        /// <param name="sortBy">Field to sort by (e.g., "name", "price").</param>
        /// <param name="ascending">Indicates whether the sort order is ascending.</param>
        /// <param name="categoryId">Optional category ID to filter products by category.</param>
        /// <returns>A collection of products that match the criteria.</returns>
        /// <exception cref="ProductNotFoundException">Thrown when the products is null.</exception>
        public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken,
                                                                 string? search = null,
                                                                 string? sortBy = "Rating", 
                                                                 bool ascending = true, 
                                                                 Guid? categoryId = null)
        {
            var products = await _unitOfWork.ProductRepository.GetAll(cancellationToken);

            if(products is null) throw new ProductNotFoundException($"{nameof(products)} is null");

            if (!string.IsNullOrWhiteSpace(search))
            {
                products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList().AsReadOnly();
            }

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList().AsReadOnly();
            }

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                products = SortProducts(products, sortBy, ascending).ToList().AsReadOnly();
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

        /// <summary>
        /// Sorts the collection of products based on the provided field and order.
        /// </summary>
        /// <param name="products">The collection of products to sort.</param>
        /// <param name="sortBy">The field to sort by.</param>
        /// <param name="ascending">Indicates whether the sort should be ascending.</param>
        /// <returns>The sorted collection of products.</returns>
        private IEnumerable<Product> SortProducts(IEnumerable<Product> products, string sortBy, bool ascending)
        {
            return sortBy?.ToLower() switch
            {
                "Name" => ascending ? products.OrderBy(p => p.Name) : products.OrderByDescending(p => p.Name),
                "Price" => ascending ? products.OrderBy(p => p.Price) : products.OrderByDescending(p => p.Price),
                "StockQuantity" => ascending ? products.OrderBy(p => p.StockQuantity) : products.OrderByDescending(p => p.StockQuantity),
                "DateAdded" => ascending ? products.OrderBy(p => p.DateAdded) : products.OrderByDescending(p => p.DateAdded),
                _ => ascending ? products.OrderBy(p => p.Rating) : products.OrderByDescending(p => p.Rating)
            };
        }
    }
}
