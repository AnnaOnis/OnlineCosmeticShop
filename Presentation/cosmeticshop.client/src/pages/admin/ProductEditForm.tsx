import React, { useEffect, useState } from 'react';
import { ProductsService } from '../../apiClient/http-services/products.service';
import { CategoryService} from '../../apiClient/http-services/category.service';
import { AdminService } from '../../apiClient/http-services/admin.service';
import { CategoryResponseDto, ProductRequestDto, ProductResponseDto} from '../../apiClient/models';
import { useNavigate, useParams } from 'react-router-dom';
import "../../styles/admin/ProductEditForm.css"
import "../../styles/admin/admin-global.css"

const productsService = new ProductsService('/api');
const categoryService = new CategoryService('/api');
const adminService = new AdminService('/api');

const ProductEditForm: React.FC = () => {
  const { productId } = useParams<{ productId: string }>();
  const [product, setProduct] = useState<ProductResponseDto | null>(null);
  const [categories, setCategories] = useState<CategoryResponseDto[] | null>(null);
  const [categoryId, setCategoryId] = useState<string>('');
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [manufacturer, setManufacturer] = useState('');
  const [price, setPrice] = useState<number>(0);
  const [stockQuantity, setStockQuantity] = useState<number>(0);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchProduct = async () => {
        if (!productId){
            setErrorMessage('Где-то косяк с Id товара');
        }else{
            try {
                const response = await productsService.getProductById(productId, new AbortController().signal);
                setProduct(response);
                setName(response.name);
                setDescription(response.description);
                setManufacturer(response.manufacturer);
                setPrice(response.price);
                setStockQuantity(response.stockQuantity);
                setCategoryId(response.categoryId);
            } catch (error) {
                console.error(error);
                setErrorMessage('Ошибка при загрузке товара');
            }            
        }
    };

    const fetchCategories = async () => {
        try{
            const response = await categoryService.getAllCategories(new AbortController().signal);
            setCategories(response);            
        }catch(error){
            console.error(error);
            setErrorMessage('Ошибка при загрузке категорий');            
        }
    }
 
    fetchProduct();
    fetchCategories();

  }, [productId]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!product) return;

    const updateData: ProductRequestDto = {
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
      await adminService.updateProduct(product.id, updateData, new AbortController().signal);
      setSuccessMessage('Товар успешно обновлен!');
      setTimeout(() => setSuccessMessage(null), 2000);
    } catch (error) {
      console.error('Ошибка при обновлении товара:', error);
      setErrorMessage('Ошибка при обновлении товара');
      setTimeout(() => setErrorMessage(null), 2000);
    }
  };

  const handleCategoryChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setCategoryId(e.target.value);
    };

  const handleBackToCatalog = () => {
    navigate('/admin/products');
  }

  return (
    <div className="product-edit-container">
      <h1 className="admin-title">Редактирование товара</h1>
      {errorMessage && <div className="message error">{errorMessage}</div>}
      {successMessage && <div className="message success">{successMessage}</div>}
      {product && (
        <form className="product-edit-form" onSubmit={handleSubmit}>
            <div className="admin-form-group">
                <label>Категория:</label>
                {categories && categories.length > 0 && ( // Проверяем, что категории загружены
                    <select
                        className="category-select"
                        value={categoryId || ''} // Значение выбранной категории
                        onChange={handleCategoryChange} // Обработчик изменения категории
                    >
                        {categories.map((category) => (
                            <option key={category.id} value={category.id}>
                                {category.categoryName}
                            </option>
                        ))}
                    </select>
                )}
            </div>
          <div className="admin-form-group">
            <label>Название:</label>
            <input
              type="text"
              value={name}
              onChange={e => setName(e.target.value)}
              className="admin-form-input"
            />
          </div>
          <div className="admin-form-group">
            <label>Описание:</label>
            <input
              type="text"
              value={description}
              onChange={e => setDescription(e.target.value)}
              className="admin-form-input"
            />
          </div>
          <div className="admin-form-group">
            <label>Производитель:</label>
            <input
              type="text"
              value={manufacturer}
              onChange={e => setManufacturer(e.target.value)}
              className="admin-form-input"
            />
          </div>
          <div className="admin-form-group">
            <label>Цена:</label>
            <input
              type="number"
              value={price}
              onChange={e => setPrice(parseFloat(e.target.value))}
              className="admin-form-input"
            />
          </div>
          <div className="admin-form-group">
            <label>Количество на складе:</label>
            <input
              type="number"
              value={stockQuantity}
              onChange={e => setStockQuantity(parseInt(e.target.value, 10))}
              className="admin-form-input"
            />
          </div>
          <button type="submit" className="btn btn-primary">Сохранить</button>
        </form>
      )}
      <button className="btn btn-primary" onClick={handleBackToCatalog}>
        Вернуться в каталог
      </button>
    </div>
  );
};

export default ProductEditForm;