import React, { useEffect, useState } from 'react';
import { CategoryService } from '../../apiClient/http-services/category.service';
import { AdminService } from '../../apiClient/http-services/admin.service';
import { CategoryResponseDto, ProductRequestDto } from '../../apiClient/models';
import { useNavigate } from 'react-router-dom';
import "../../styles/admin/ProductEditForm.css"

const categoryService = new CategoryService('/api');
const adminService = new AdminService('/api');

const ProductAddForm: React.FC = () => {
  const [categories, setCategories] = useState<CategoryResponseDto[] | null>(null);
  const [categoryId, setCategoryId] = useState<string>('');
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [manufacturer, setManufacturer] = useState('');
  const [price, setPrice] = useState<number>(0);
  const [stockQuantity, setStockQuantity] = useState<number>(0);
  // const [image, setImage] = useState<File | null>(null);
  // const [imageUrl, setImageUrl] = useState<string | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await categoryService.getAllCategories(new AbortController().signal);
        setCategories(response);
        if(response && response.length > 0 && response[0].id !== undefined){
          setCategoryId(response[0].id);          
        }

      } catch (error) {
        console.error(error);
        setErrorMessage('Ошибка при загрузке категорий');
      }
    };

    fetchCategories();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const productData: ProductRequestDto = {
      name,
      description,
      categoryId,
      price,
      rating: 0,
      manufacturer,
      stockQuantity,
      imageUrl: ''
    };

    try {
      await adminService.createProduct(productData, new AbortController().signal);
      setSuccessMessage('Товар успешно добавлен!');
      setTimeout(() => setSuccessMessage(null), 2000);
      navigate('/product-admin-table');
    } catch (error) {
      console.error('Ошибка при добавлении товара:', error);
      setErrorMessage('Ошибка при добавлении товара');
      setTimeout(() => setErrorMessage(null), 2000);
    }
  };

  const handleCategoryChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setCategoryId(e.target.value);
  };

  // const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
  //   if (e.target.files && e.target.files.length > 0) {
  //       setImage(e.target.files[0]);
  //   }
  // };

  const handleBackToCatalog = () => {
    navigate('/product-admin-table');
  }

  return (
    <div className="product-edit-container">
      <h1 className="admin-title">Добавление нового товара</h1>
      {errorMessage && <div className="message error">{errorMessage}</div>}
      {successMessage && <div className="message success">{successMessage}</div>}
      {categories && categories.length > 0 && (
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Категория:</label>
            <select
              className="category-select"
              value={categoryId}
              onChange={handleCategoryChange}
            >
              {categories.map((category) => (
                <option key={category.id} value={category.id}>
                  {category.categoryName}
                </option>
              ))}
            </select>
          </div>
          <div className="form-group">
            <label>Название:</label>
            <input
              type="text"
              value={name}
              onChange={e => setName(e.target.value)}
              className="form-input"
            />
          </div>
          <div className="form-group">
            <label>Описание:</label>
            <input
              type="text"
              value={description}
              onChange={e => setDescription(e.target.value)}
              className="form-input"
            />
          </div>
          <div className="form-group">
            <label>Производитель:</label>
            <input
              type="text"
              value={manufacturer}
              onChange={e => setManufacturer(e.target.value)}
              className="form-input"
            />
          </div>
          <div className="form-group">
            <label>Цена:</label>
            <input
              type="number"
              value={price}
              onChange={e => setPrice(parseFloat(e.target.value))}
              className="form-input"
            />
          </div>
          <div className="form-group">
            <label>Количество на складе:</label>
            <input
              type="number"
              value={stockQuantity}
              onChange={e => setStockQuantity(parseInt(e.target.value, 10))}
              className="form-input"
            />
          </div>
          {/* <div className="form-group">
            <label>Изображение:</label>
            <input
              type="file"
              accept="image/*"
              onChange={handleImageChange}
              className="form-input"
            />
          </div> */}
          <button type="submit" className="btn btn-primary">Добавить товар</button>
        </form>
      )}
      <button className="btn btn-primary" onClick={handleBackToCatalog}>
        Вернуться в каталог
      </button>
    </div>
  );
};

export default ProductAddForm;