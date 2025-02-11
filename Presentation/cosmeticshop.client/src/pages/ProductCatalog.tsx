import React, { useEffect, useState } from 'react';
import { ProductsService } from '../apiClient/http-services/products.service';
import { ProductResponseDto, FilterDto } from '../apiClient/models';
import { CartService } from '../apiClient/http-services/cart.service';
import { CartItemRequestDto } from '../apiClient/models';
import { useAuth } from '../context/AuthContext';
import ProductCard from '../components/ProductCard';
import { useCart } from '../context/CartContext'; 
import { FavoriteService } from '../apiClient/http-services/favorite.service';
import { FavoriteRequestDto} from '../apiClient/models';
import '../styles/ProductCatalog.css';

const ProductCatalog: React.FC = () => {
  const [products, setProducts] = useState<ProductResponseDto[]>([]);
  const [filterDto, setFilterDto] = useState<FilterDto>({
    filter: '',
    sortField: '',
    sortOrder: true,
    pageNumber: 1,
    pageSize: 10
  });
  const [totalPages, setTotalPages] = useState<number>(1);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [favoriteProducts, setFavoriteProducts] = useState<Set<string>>(new Set());
  const productsService = new ProductsService('/api');
  const cartService = new CartService('/api');
  const favoriteService = new FavoriteService('/api');
  const { isAuthenticated, id: customerId} = useAuth();
  const { dispatch } = useCart();

  useEffect(() => {
    const fetchProducts = async () => {
      console.log('Fetching products with:', filterDto); // Логирование для отладки
      try {
        const response = await productsService.getProducts({
          ...filterDto,
          filter: filterDto.filter.toLowerCase()
      }, new AbortController().signal);
        setProducts(response.items);
        setTotalPages(Math.ceil(response.totalItems / filterDto.pageSize));
      } catch (error) {
        console.error(error);
      }
    };

    const fetchFavorites = async () => {
      try {
        if(isAuthenticated && customerId){
          const response = await favoriteService.getAllFavoritesByCustomerId(customerId, new AbortController().signal);
          setFavoriteProducts(new Set(response.map((product: ProductResponseDto) => product.id)));          
        }
      } catch (error) {
        console.error(error);
      }
    };

    fetchFavorites();
    fetchProducts();
    
  }, [filterDto]);

  const handleFilterChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFilterDto({
      ...filterDto,
      filter: e.target.value
    });
  };

  const handleSortFieldChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setFilterDto({
      ...filterDto,
      sortField: e.target.value
    });
  };

  const handleSortOrderChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setFilterDto({
      ...filterDto,
      sortOrder: e.target.value === "asc"
    });
  };

  const handlePageChange = (pageNumber: number) => {
    setFilterDto({
      ...filterDto,
      pageNumber
    });
  };

  const handlePageSizeChange = (e: React.ChangeEvent<HTMLSelectElement>) =>{
    setFilterDto({
      ...filterDto,
      pageSize: parseInt(e.target.value, 10)
    });
  };

  const handleAddToCart = async (productId: string) => {
    if (!isAuthenticated) {
      setErrorMessage('Чтобы добавить товары в корзину ВОЙДИТЕ или ЗАРЕГИСТРИРУЙТЕСЬ!');
      setTimeout(() => setErrorMessage(null), 2000);
      return;
    }

    try {
      const body: CartItemRequestDto = {
      productId: productId,
      quantity: 1};
      
      await cartService.addItemToCart(body, new AbortController().signal);
      setSuccessMessage('Товар успешно добавлен в корзину!');
      setTimeout(() => setSuccessMessage(null), 2000);

      const updatedCart = await cartService.getCart(new AbortController().signal);
      // Обновляем состояние корзины
      dispatch({ type: 'SET_CART', payload: updatedCart });
    } catch (error) {
      setErrorMessage('Ошибка при добавлении товара в корзину');
      setTimeout(() => setErrorMessage(null), 2000);
    }
  };

  const handleAddOrRemoveProductToFavorites = async (event: React.MouseEvent<HTMLButtonElement>, productId: string) => {
    event.stopPropagation(); // Предотвращаем распространение клика
    if (!customerId) {
      setErrorMessage('Чтобы добавить товары в избранное ВОЙДИТЕ или ЗАРЕГИСТРИРУЙТЕСЬ!');
      setTimeout(() => setErrorMessage(null), 2000);
      return;
    }

    try {
      const request: FavoriteRequestDto = {
        productId: productId,
        customerId: customerId
      };

      if (favoriteProducts.has(productId)) {
        // Удаляем товар из избранного
        await favoriteService.removeFromFavorites(productId, customerId, new AbortController().signal);
        setFavoriteProducts(prev => new Set([...prev].filter(id => id !== productId)));
      } else {
        // Добавляем товар в избранное
        await favoriteService.addToFavorites(request, new AbortController().signal);
        setFavoriteProducts(prev => new Set([...prev, productId]));
      }
    } catch (error) {
      console.error("Ошибка при добавлении/удалении товара из избранного", error);
      setErrorMessage('Ошибка при добавлении/удалении товара из избранного');
      setTimeout(() => setErrorMessage(null), 2000);
    }
  };

  return (
    <div className="catalog-container">
      <div className="catalog-header">
        <h1 className="catalog-title">Каталог товаров</h1>
        
        <div className="filters-container">
          <div className="filter-group">
            <input
              type="text"
              placeholder="Поиск товаров..."
              className="search-input"
              value={filterDto.filter}
              onChange={handleFilterChange}
            />
          </div>

          <div className="filter-group">
            <select 
              className="filter-select"
              value={filterDto.sortField} 
              onChange={handleSortFieldChange}
            >
              <option value="">Сортировать по</option>
              <option value="Rating">Рейтингу</option>
              <option value="Name">Названию</option>
              <option value="Price">Цене</option>
            </select>

            <select
              className="filter-select"
              value={filterDto.sortOrder ? "asc" : "desc"}
              onChange={handleSortOrderChange}
            >
              <option value="asc">По возрастанию</option>
              <option value="desc">По убыванию</option>
            </select>

            <select
              className="filter-select"
              value={filterDto.pageSize}
              onChange={handlePageSizeChange}
            >
              <option value="10">10 на странице</option>
              <option value="25">25 на странице</option>
              <option value="50">50 на странице</option>
            </select>
          </div>
        </div>
      </div>

      {errorMessage && <div className="message error">{errorMessage}</div>}
      {successMessage && <div className="message success">{successMessage}</div>}

      <div className="products-grid">
        {products.map((product) => (
          <ProductCard
            key={product.id}
            product={{
              id: product.id,
              categoryId: product.categoryId,
              name: product.name,
              price: product.price,
              stockQuantity: product.stockQuantity,
              imageUrl: product.imageUrl,
              description: product.description,
              manufacturer: product.manufacturer,
              rating: product.rating
            }}
            onAddToCart={() => handleAddToCart(product.id)}
            isAuthenticated={isAuthenticated}
            onAddOrRemoveProductToFavorites={handleAddOrRemoveProductToFavorites}
            isFavorited={favoriteProducts.has(product.id)}
          />
        ))}
      </div>

      <div className="pagination">
        {Array.from({ length: totalPages }, (_, i) => (
          <button
            key={i + 1}
            className={`page-btn ${i + 1 === filterDto.pageNumber ? 'active' : ''}`}
            onClick={() => handlePageChange(i + 1)}
          >
            {i + 1}
          </button>
        ))}
      </div>
    </div>
  );
};

export default ProductCatalog;