import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { ProductsService} from '../apiClient/http-services/products.service';
import { ReviewsService } from '../apiClient/http-services/reviews.service';
import { ProductResponseDto, ReviewResponseDto } from '../apiClient/models';
import { CartService } from '../apiClient/http-services/cart.service';
import { CartItemRequestDto } from '../apiClient/models';
import { useAuth } from '../AuthContext';
import { Link } from 'react-router-dom';

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

  if (!product) {
    return <div>Загрузка...</div>;
  }

  return (
    <div>
      <h1>{product.name}</h1>
      <img src={product.imageUrl} alt={product.name} className="product-image"/>
      <p>{product.description}</p>
      <p>Цена: {product.price} руб.</p>
      <p>Рейтинг: {product.rating}</p>
      <button onClick={handleAddToCartClick}>Добавить в корзину</button>
      <h2>Отзывы</h2>
      {reviews ? 
      (<div className="reviews">
        {reviews.map((review, index) => (
          <div key={index} className="review-item">
            <p><strong>{new Date(review.reviewDate).toLocaleDateString('ru-RU', { day: '2-digit', month: '2-digit', year: 'numeric' })}</strong></p>
            <p><strong>{review.customerName}</strong>: {review.reviewText}</p>
            <p>Рейтинг: {review.rating}</p>
          </div>
        ))}
      </div>) : (<div>Загрузка...</div>)}
      {successMessage && <div className="success-message" dangerouslySetInnerHTML={{ __html: successMessage }} />}
      {errorMessage && <div className="error-message">{errorMessage}</div>}
    </div>
  );
};

export default ProductDetails;