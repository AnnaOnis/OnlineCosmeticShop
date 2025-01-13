import React, { useEffect, useState } from 'react';
import { ProductsService } from '../apiClient/http-services/products.service';
import { ProductResponseDto, FilterDto } from '../apiClient/models';
import { Pagination } from 'react-bootstrap';

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
  const productsService = new ProductsService('/api');

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
            <option value="Name">Названию</option>
            <option value="Price">Цене</option>
            <option value="Rating">Рейтингу</option>
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