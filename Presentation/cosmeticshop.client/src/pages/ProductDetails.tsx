import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { ProductsService} from '../apiClient/http-services/products.service';
import { ReviewsService } from '../apiClient/http-services/reviews.service';
import { ProductResponseDto, ReviewResponseDto } from '../apiClient/models';
import { CartService } from '../apiClient/http-services/cart.service';
import { CartItemRequestDto } from '../apiClient/models';
import { useAuth } from '../context/AuthContext';
import { useCart } from '../context/CartContext';
import { Link } from 'react-router-dom';
import '../styles/ProductDetails.css'

const ProductDetails: React.FC = () => {
  const { id } = useParams<{ id: string | undefined }>();
  const [product, setProduct] = useState<ProductResponseDto | null>(null);
  const [reviews, setReviews] = useState<ReviewResponseDto[] | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const productsService = new ProductsService('/api');
  const reviewService = new ReviewsService('/api');
  const cartService = new CartService('/api');
  const { isAuthenticated } = useAuth();
  const { dispatch } = useCart();

  useEffect(() => {
    if (id) {
      const fetchProduct = async () => {
        try {
          const productResponse = await productsService.getProductById(id, new AbortController().signal);
          setProduct(productResponse);

          const reviewsResponse = await reviewService.getApprovedReviewsByProduct(id, new AbortController().signal);
          setReviews(reviewsResponse);
        } catch (error) {
          console.error(error);
        }
      };

      fetchProduct();
    }
  }, [id]);

  const handleAddToCartClick = async () => {
    if (!isAuthenticated) {
      setErrorMessage('Чтобы добавить товары в корзину ВОЙДИТЕ или ЗАРЕГИСТРИРУЙТЕСЬ!');
      setSuccessMessage(null);
      setTimeout(() => setErrorMessage(null), 2000); // Сообщение исчезнет через 3 секунды
    } else {
      if (!product) return;

      const body: CartItemRequestDto = {
        productId: product.id,
        quantity: 1
      };

      try {
        const response = await cartService.addItemToCart(body, new AbortController().signal);
        console.log('Товар добавлен в корзину:', response);
        setSuccessMessage('Товар успешно добавлен в корзину!');
        setErrorMessage(null);
        setTimeout(() => setSuccessMessage(null), 2000); // Сообщение исчезнет через 3 секунды

        const updatedCart = await cartService.getCart(new AbortController().signal);
        // Обновляем состояние корзины
        dispatch({ type: 'SET_CART', payload: updatedCart });
        
      } catch (error) {
        console.error('Ошибка при добавлении товара в корзину:', error);
        setErrorMessage('Ошибка при добавлении товара в корзину. Пожалуйста, попробуйте снова.');
        setSuccessMessage(null);
        setTimeout(() => setErrorMessage(null), 2000); // Сообщение исчезнет через 3 секунды
      }
    }
  };

  if (!product) {
    return <div>Загрузка...</div>;
  }

  return (
    <div className="product-details-container">
      <div className="product-main">
        <div className="product-gallery">
          <img 
            src={product.imageUrl} 
            alt={product.name} 
            className="main-image"
          />
        </div>

        <div className="product-info">
          <h1 className="product-title">{product.name}</h1>
          <div className="product-meta">
            <span className="product-rating">★ {product.rating}</span>
            <span className="product-sku">Артикул: {product.id}</span>
          </div>

          <p className="product-price">{product.price} ₽</p>
          <p className="product-description">{product.description}</p>

          <button 
            className="add-to-cart-btn"
            onClick={handleAddToCartClick}
          >
            Добавить в корзину
          </button>
        </div>
      </div>

      <div className="reviews-section">
        <h2 className="section-title">Отзывы ({reviews?.length || 0})</h2>
        
        <div className="reviews-list">
          {reviews?.map((review) => (
            <div key={review.id} className="review-card">
              <div className="review-header">
                <div className="user-avatar">
                  {review.customerName}
                </div>
                <div className="user-info">
                  <h4>{review.customerName}</h4>
                  <div className="review-meta">
                    <span className="review-rating">★ {review.rating}</span>
                    <span className="review-date">
                      {new Date(review.reviewDate).toLocaleDateString('ru-RU')}
                    </span>
                  </div>
                </div>
              </div>
              <p className="review-text">{review.reviewText}</p>
            </div>
          ))}
          
          {!reviews?.length && (
            <p className="no-reviews">Пока нет отзывов. Будьте первым!</p>
          )}
        </div>
      </div>

      {/* Сообщения об ошибках и успехе */}
      {successMessage && (
        <div className="message success">
          {successMessage}
          <Link to="/cart">Перейти в корзину</Link>
        </div>
      )}
      {errorMessage && <div className="message error">{errorMessage}</div>}
    </div>
  );
};

export default ProductDetails;