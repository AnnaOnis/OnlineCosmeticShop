import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { CartItemResponseDto } from '../apiClient/models/cart-item-response-dto';
import '../styles/OrderConfirmation.css'

const OrderConfirmation: React.FC = () => {
  const location = useLocation();
  const order = location.state?.order;
  const navigate = useNavigate();

  if (!order) {
    return (
      <div>
        <h1>Заказ не найден</h1>
        <p>Не удалось получить данные о заказе.</p>
        <button onClick={() => (window.location.href = '/')}>На главную</button>
      </div>
    );
  }

  return (
    <div className="confirmation-container">
      <div className="confirmation-card">
        <h1 className="confirmation-title">Заказ успешно оформлен! 🎉</h1>
        <div className="confirmation-icon">✓</div>
        
        <div className="confirmation-order-summary">
          <p className="confirmation-order-number">Номер заказа: #{order.id}</p>
          <p className="confirmation-notification-text">
            Подробности заказа были отправлены на вашу электронную почту.
          </p>
  
          <div className="confirmation-order-details">
            <div className="confirmation-detail-section">
              <h3>Состав заказа:</h3>
              <div className="confirmation-items-list">
                {order.orderItems.map((item: CartItemResponseDto) => (
                  <div key={item.productId} className="confirmation-order-item">
                    <span>{item.productName}</span>
                    <span>{item.quantity} x {item.productPrice} ₽</span>
                  </div>
                ))}
              </div>
            </div>
  
            <div className="confirmation-total-section">
              <h3>Сумма заказа:</h3>
              <p className="confirmation-total-amount">{order.totalAmount.toLocaleString()} ₽</p>
            </div>
  
            <div className="method-info">
              <div>
                <h3>Способ доставки:</h3>
                <p>{order.orderShippingMethod === 0 ? 'Почта России' : 
                   order.orderShippingMethod === 1 ? 'Курьерская доставка' : 'Самовывоз'}</p>
              </div>
              <div>
                <h3>Способ оплаты:</h3>
                <p>{order.orderPaymentMethod === 0 ? 'Банковской картой онлайн' : 
                   order.orderPaymentMethod === 1 ? 'Наличными при получении' : 'Не указан'}</p>
              </div>
            </div>
          </div>
        </div>
  
        <button 
          className="home-button"
          onClick={() => navigate('/')}
        >
          Вернуться на главную
        </button>
      </div>
    </div>
  );
};

export default OrderConfirmation;