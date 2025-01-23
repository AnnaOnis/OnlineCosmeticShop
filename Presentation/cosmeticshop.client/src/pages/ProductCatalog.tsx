import React, { useEffect, useState } from 'react';
import { ProductsService } from '../apiClient/http-services/products.service';
import { ProductResponseDto, FilterDto } from '../apiClient/models';
import { Pagination } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { CartService } from '../apiClient/http-services/cart.service';
import { CartItemRequestDto } from '../apiClient/models';
import { useAuth } from '../AuthContext';

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
  const productsService = new ProductsService('/api');
  const cartService = new CartService('/api');
  const { isAuthenticated} = useAuth();

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

  const handleAddToCartClick = async (e: React.MouseEvent<HTMLButtonElement>) => {
    e.stopPropagation(); // Предотвращаем распространение события клика
    if(!isAuthenticated){
      setErrorMessage('Чтобы добавить товары в корзину ВОЙДИТЕ или ЗАРЕГИСТРИРУЙТЕСЬ!');
      setSuccessMessage(null);
      setTimeout(() => setErrorMessage(null), 2000); // Сообщение исчезнет через 3 секунды
    }
    else{
      const productId = e.currentTarget.getAttribute('data-product-id')!;
      const body: CartItemRequestDto = {
        productId: productId,
        quantity: 1
      };

      try {
        const response = await cartService.addItemToCart(body, new AbortController().signal);
        console.log('Товар добавлен в корзину:', response);
        setSuccessMessage('Товар успешно добавлен в корзину!' + <Link to="/cart">Перейти в корзину для просмотра.</Link>);
        setErrorMessage(null);
        setTimeout(() => setSuccessMessage(null), 2000); // Сообщение исчезнет через 3 секунды
      } catch (error) {
        console.error('Ошибка при добавлении товара в корзину:', error);
        setErrorMessage('Ошибка при добавлении товара в корзину. Пожалуйста, попробуйте снова.');
        setSuccessMessage(null);
        setTimeout(() => setErrorMessage(null), 2000); // Сообщение исчезнет через 3 секунды
      }      
    }
  };

  return (
    <div>
      <h1>Каталог товаров</h1>

      {/* Фильтры и сортировка товаров */}
      <div className="filters">
        <div>
          <label>Фильтр:</label>
          <input type="text" value={filterDto.filter} onChange={handleFilterChange} />
        </div>
        <div>
          <label>Сортировка по:</label>
          <select value={filterDto.sortField} onChange={handleSortFieldChange}>
            <option value="Rating">Рейтингу</option>
            <option value="Name">Названию</option>
            <option value="Price">Цене</option>
          </select>
        </div>
        <div>
          <label>Порядок:</label>
          <select value={filterDto.sortOrder ? "asc" : "desc"} onChange={handleSortOrderChange}>
            <option value="asc">По возрастанию</option>
            <option value="desc">По убыванию</option>
          </select>
        </div>
        <div>
          <label>Товаров на странице:</label>
          <select value={filterDto.pageSize} onChange={handlePageSizeChange}>
            <option value="10">10</option>
            <option value="25">25</option>
            <option value="50">50</option>
            <option value="100">100</option>
          </select>
        </div>
      </div>

      {successMessage && <div className="success-message" dangerouslySetInnerHTML={{ __html: successMessage }} />}
      {errorMessage && <div className="error-message">{errorMessage}</div>}

      <div className="product-list">
        {products.map((product) => (
          <div key={product.id} className="product-item">
            <Link to={`/product/${product.id}`} className="productLink">
              <img src={product.imageUrl} alt={product.name} />
              <h2>{product.name}</h2>
              <p>{product.description}</p>
              <p>Цена: {product.price} руб.</p>
              <p>Рейтинг: {product.rating}</p>
            </Link>
              <button onClick={handleAddToCartClick} data-product-id={product.id}>
                Добавить в корзину
              </button>             
          </div>        
        ))}
      </div>

      {/* Пагинация */}
      <div className="pagination">
        <Pagination>
          <Pagination.First onClick={() => handlePageChange(1)} disabled={filterDto.pageNumber === 1} />
          <Pagination.Prev onClick={() => handlePageChange(filterDto.pageNumber - 1)} disabled={filterDto.pageNumber === 1} />
          {Array.from({ length: totalPages }, (_, i) => (
            <Pagination.Item
              key={i + 1}
              active={i + 1 === filterDto.pageNumber}
              onClick={() => handlePageChange(i + 1)}
            >
              {i + 1}
            </Pagination.Item>
          ))}
          <Pagination.Next onClick={() => handlePageChange(filterDto.pageNumber + 1)} disabled={filterDto.pageNumber === totalPages} />
          <Pagination.Last onClick={() => handlePageChange(totalPages)} disabled={filterDto.pageNumber === totalPages} />
        </Pagination>
      </div>
    </div>
  );
};

export default ProductCatalog;