import React, { useEffect, useState } from 'react';
import { ProductsService } from '../apiClient/http-services/products.service';
import { ProductResponseDto, FilterDto } from '../apiClient/models';
import { Pagination } from 'react-bootstrap';

const ProductCatalog: React.FC = () => {
  const [products, setProducts] = useState<ProductResponseDto[]>([]);
  const [filterDto, setFilterDto] = useState<FilterDto>({
    filter: '',
    sortField: 'name',
    sortOrder: true,
    pageNumber: 1,
    pageSize: 10
  });
  const [totalPages, setTotalPages] = useState<number>(1);
  const productsService = new ProductsService('/api');

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const response = await productsService.getProducts(filterDto, new AbortController().signal);
        setProducts(response);
        // Предполагается, что сервер возвращает общее количество страниц
        // Например, в заголовке ответа или в теле ответа
        // Здесь мы просто устанавливаем totalPages в 10 для примера
        setTotalPages(10);
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
      sortOrder: e.target.value == "asc"
    });
  };

  const handlePageChange = (pageNumber: number) => {
    setFilterDto({
      ...filterDto,
      pageNumber
    });
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
            <option value="name">Названию</option>
            <option value="price">Цене</option>
            <option value="rating">Рейтингу</option>
          </select>
        </div>
        <div>
          <label>Порядок:</label>
          <select value={filterDto.sortOrder ? "asc" : "desc"} onChange={handleSortOrderChange}>
            <option value="asc">По возрастанию</option>
            <option value="desc">По убыванию</option>
          </select>
        </div>
      </div>

      <div className="product-list">
        {products.map((product) => (
          <div key={product.id} className="product-item">
            <img src={product.imageUrl} alt={product.name} />
            <h2>{product.name}</h2>
            <p>{product.description}</p>
            <p>Цена: {product.price} руб.</p>
            <button>Добавить в корзину</button>
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