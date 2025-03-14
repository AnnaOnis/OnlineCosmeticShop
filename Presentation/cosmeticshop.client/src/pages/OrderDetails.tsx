import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { OrderService } from '../apiClient/http-services/order.service';
import { OrderResponseDto, ReviewCreateRequestDto} from '../apiClient/models';
import '../styles/OrderDetails.css';
import { ReviewsService } from '../apiClient/http-services/reviews.service';
import { orderPaymentMethodMap, 
         orderPaymentStatusMap, 
         orderShippingMethodMap, 
         orderStatusMap } from '../apiClient/models/map/modelMaps';

const OrderDetails: React.FC = () => {
  const { orderId } = useParams<{ orderId: string }>();
  const [order, setOrder] = useState<OrderResponseDto | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [showReviewForm, setShowReviewForm] = useState<string | null>(null);
  const [reviewText, setReviewText] = useState('');
  const [rating, setRating] = useState(5);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [expandedReviews, setExpandedReviews] = useState<{ [key: string]: boolean }>({});
  const reviewService = new ReviewsService('/api');

  const orderService = new OrderService('/api');

    const fetchOrder = async () => {
      if (!orderId) {
        setError('Неверный идентификатор заказа');
        return;
      }
      try {
        const response = await orderService.getOrderDetailsAsync(orderId, new AbortController().signal);
        setOrder(response);
      } catch (err) {
        setError('Ошибка при загрузке заказа');
      } finally {
        setLoading(false);
      }
    };  

  useEffect(() => {

    fetchOrder();

  }, [orderId]);

  if (loading) {
    return <div>Загрузка...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  if (!order) {
    return <div>Заказ не найден</div>;
  }

  const validateForm = () => {
    if (reviewText.trim().length === 0) {
      setErrorMessage('Пожалуйста, напишите отзыв.');
      setTimeout(() => setErrorMessage(null), 2000);
      return false;
    }
    if (rating < 1 || rating > 5) {
        setErrorMessage('Пожалуйста, выберите рейтинг от 1 до 5.');
        setTimeout(() => setErrorMessage(null), 2000);
      return false;
    }
    setErrorMessage(null);
    return true;
  };

  const handleSubmitReview = async (productId: string) => {
    if (!orderId) return;
    if (!validateForm()) return;

    const reviewData: ReviewCreateRequestDto = {
      productId,      
      reviewText,
      rating,
    };

    try {
      await reviewService.createReview(reviewData, new AbortController().signal);
      setShowReviewForm(null);
      setSuccessMessage('Отзыв успешно отправлен!');
      setTimeout(() => setSuccessMessage(null), 2000);
      fetchOrder();
    } catch (error) {
      console.error('Ошибка при отправке отзыва:', error);
    }
  };

  const handlePayOrder = async () => {
    if (!orderId) return;
  
    try {
      const order = await orderService.orderPaymentProcessing(orderId, new AbortController().signal);
      setOrder(order);
      if(order.orderPaymentStatus == 1)
      {
        setSuccessMessage('Заказ успешно оплачен!');
        setTimeout(() => setSuccessMessage(null), 2000);        
      }else{
        setErrorMessage('Не удалось оплатить заказ! Попробуйте еще раз.');
        setTimeout(() => setErrorMessage(null), 2000);
      }

    } catch (error) {
      console.error('Ошибка при обработке оплаты:', error);
      setErrorMessage('Ошибка при обработке оплаты.');
      setTimeout(() => setErrorMessage(null), 2000);
    }
  };

  const toggleReviews = (productId: string) => {
    setExpandedReviews((prev) => ({
      ...prev,
      [productId]: !prev[productId],
    }));
  };


  return (
    <div className="order-details-container">
      <h1>Детали заказа No: {order.id}</h1>
      <div>
        <div className="order-details-info-group">
          <span className="order-details-info-label">Статус:</span>
          <span className="order-details-info-value">{orderStatusMap[order.status]}</span>
        </div>
        <div className="order-details-info-group">
          <span className="order-details-info-label">Дата заказа:</span>
          <span className="order-details-info-value">{new Date(order.orderDate).toLocaleDateString('ru-RU')}</span>
        </div>
        <div className="order-details-info-group">
          <span className="order-details-info-label">Сумма заказа:</span>
          <span className="order-details-info-value">{order.totalAmount} ₽</span>
        </div>
        <div className="order-details-info-group">
          <span className="order-details-info-label">Способ оплаты:</span>
          <span className="order-details-info-value">{orderPaymentMethodMap[order.orderPaymentMethod]}</span>
        </div>
        <div className="order-details-info-group">
          <span className="order-details-info-label">Статус оплаты:</span>
          <span className="order-details-info-value">{orderPaymentStatusMap[order.orderPaymentStatus]}</span>
          {order.orderPaymentStatus === 0 || order.orderPaymentStatus === 2 ? (
          <button className="btn btn-primary" onClick={handlePayOrder}>
            Оплатить
          </button>
        ) : null}
        </div>
        <div className="order-details-info-group">
          <span className="order-details-info-label">Способ доставки:</span>
          <span className="order-details-info-value">{orderShippingMethodMap[order.orderShippingMethod]}</span>
        </div>
        <div className="order-details-info-group">
            <span className="order-details-info-label">Товары:</span>
            <div className="order-details-items-list">
        {order.orderItems.map((item) => (
          <div key={item.productId} className="order-details-item-card">
            <div className="order-details-product-info">
              <img 
                src={item.productImageUrl || '/images/placeholder.jpg'} 
                alt={item.productName}
                className="order-details-product-image"
              />
              <div className="order-details-product-details">
                <h3>{item.productName}</h3>
                <div className="order-details-price-quantity">
                  <span>{item.quantity} × {item.productPrice} ₽</span>
                  <span className="order-details-total-price">{(item.quantity * item.productPrice).toLocaleString()} ₽</span>
                </div>
              </div>
            </div>

            {errorMessage && <div className="message error">{errorMessage}</div>}
            {successMessage && <div className="message success">{successMessage}</div>}

            <div className="order-details-review-section">
                    {showReviewForm === item.productId ? (
                    <div className="order-details-review-form">
                        <div className="order-details-rating-stars">
                        {[1, 2, 3, 4, 5].map((star) => (
                            <button
                            key={star}
                            className={`order-details-star ${star <= rating ? 'active' : ''}`}
                            onClick={() => setRating(star)}
                            type="button"
                            >
                            ★
                            </button>
                        ))}
                        </div>
                        <textarea
                        className="order-details-review-textarea"
                        value={reviewText}
                        onChange={(e) => setReviewText(e.target.value)}
                        placeholder="Расскажите о вашем опыте использования товара"
                        />
                        <div className="order-details-form-actions">
                        <button 
                            className="btn btn-primary"
                            onClick={() => handleSubmitReview(item.productId)}
                        >
                            Отправить отзыв
                        </button>
                        <button
                            className="btn btn-outline"
                            onClick={() => setShowReviewForm(null)}
                        >
                            Отмена
                        </button>
                        </div>
                        <br/>
                    </div>
                    ) : (
                      <>                     
                        <div>
                          <button 
                              className="btn btn-outline"
                              onClick={() => setShowReviewForm(item.productId)}>                   
                              Добавить отзыв
                          </button>                       
                        </div><br/>                     
                      </>
                    )}
              </div>
              {item.reviews.length > 0 && (
                <div className="order-details-review-section">
                  <div className='review-header'
                    onClick={() => toggleReviews(item.productId)}>
                    <h3>Ваши отзывы</h3>
                    <i className={`fas fa-${expandedReviews[item.productId] ? 'angle-up' : 'angle-down'}`}></i>
                  </div>
                  {expandedReviews[item.productId] && (
                    <div className="order-details-review-text">
                      {item.reviews.map((r) => (
                        <>
                          <hr/><br/>
                          <p>{new Date(r.reviewDate).toLocaleDateString('ru-RU')}</p><br/>
                          <p>{r.reviewText}</p>
                          <p>Оценка: {r.rating} <span className="order-details-star active">★</span></p>
                          <p>Статус: {r.isApproved ? 'одобрен' : 'на модерации'}</p>  
                        </>
                      ))}
                    </div> 
                  )}
                </div>
              )}           
          </div>
        ))}
      </div>
    </div>
        </div>
      </div>
  );
};

export default OrderDetails;