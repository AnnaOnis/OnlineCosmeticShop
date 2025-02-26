import React, { useEffect, useState } from 'react';
import { ProductsService } from '../../apiClient/http-services/products.service';
import { AdminService } from '../../apiClient/http-services/admin.service';
import { ProductResponseDto } from '../../apiClient/models';
import { useNavigate } from 'react-router-dom';
import "../../styles/admin/ProductAdminTable.css"
import "../../styles/admin/admin-global.css"

const productsService = new ProductsService('/api');
const adminService = new AdminService('/api');

const ProductAdminTable: React.FC = () => {
  const [products, setProducts] = useState<ProductResponseDto[]>([]);
  const [filterDto, setFilterDto] = useState({
    filter: '',
    sortField: '',
    sortOrder: true,
    pageNumber: 1,
    pageSize: 10
  });
  const [totalPages, setTotalPages] = useState<number>(1);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const response = await productsService.getProducts(filterDto, new AbortController().signal);
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
      sortOrder: e.target.value === 'asc' ? true : false
    });
  };

  const handlePageChange = (pageNumber: number) => {
    setFilterDto({
      ...filterDto,
      pageNumber
    });
  };

  const handlePageSizeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setFilterDto({
      ...filterDto,
      pageSize: parseInt(e.target.value, 10)
    });
  };

  const handleEditProduct = (productId: string) => {
    navigate(`/admin/product-edit/${productId}`);
  };

  const handleDeleteProduct = async (productId: string) => {
    if (!confirm('Вы уверены, что хотите удалить этот товар?')) {
      return;
    }

    try {
      await adminService.deleteProduct(productId, new AbortController().signal);
      setProducts(products.filter(product => product.id !== productId));
      setSuccessMessage('Товар успешно удален!');
      setTimeout(() => setSuccessMessage(null), 2000);
    } catch (error) {
      console.error('Ошибка при удалении товара:', error);
      setErrorMessage('Не удалось удалить товар!');
      setTimeout(() => setErrorMessage(null), 2000);
    }
  };

  return (
    <div className="admin-table-container">
      <h1 className="admin-title">Каталог товаров</h1>
      <div className="add-button">
        <button className="btn btn-primary" onClick={() => navigate('/admin/product-add')}>
          Добавить новый товар
        </button>        
      </div>

      <div className="admin-filters-container">
        <input
          type="text"
          placeholder="Поиск товаров..."
          className="admin-search-input"
          value={filterDto.filter}
          onChange={handleFilterChange}
        />
        <select
          className="admin-filter-select"
          value={filterDto.sortField}
          onChange={handleSortFieldChange}
        >
          <option value="">Сортировать по</option>
          <option value="Name">Названию</option>
          <option value="Price">Цене</option>
          <option value="StockQuantity">Количеству на складе</option>
          <option value="DateAdded">Дате добавления</option>
        </select>
        <select
          className="admin-filter-select"
          value={filterDto.sortOrder ? "asc" : "desc"}
          onChange={handleSortOrderChange}
        >
          <option value="asc">По возрастанию</option>
          <option value="desc">По убыванию</option>
        </select>
        <select
          className="admin-filter-select"
          value={filterDto.pageSize}
          onChange={handlePageSizeChange}
        >
          <option value="10">10 на странице</option>
          <option value="25">25 на странице</option>
          <option value="50">50 на странице</option>
        </select>
      </div>

      {errorMessage && <div className="message error">{errorMessage}</div>}
      {successMessage && <div className="message success">{successMessage}</div>}

      <table className="admin-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Название</th>
            <th>Цена</th>
            <th>Количество на складе</th>
            <th>Действия</th>
          </tr>
        </thead>
        <tbody>
          {products.map(product => (
            <tr key={product.id}>
              <td>{product.id}</td>
              <td>{product.name}</td>
              <td>{product.price} ₽</td>
              <td>{product.stockQuantity}</td>
              <td>
                <button className="btn btn-outline" onClick={() => handleEditProduct(product.id)}>
                    <i className="fas fa-edit"></i>
                </button>
                <button className="btn btn-outline" onClick={() => handleDeleteProduct(product.id)}>
                    <i className="fas fa-trash-alt"></i>
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
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

export default ProductAdminTable;