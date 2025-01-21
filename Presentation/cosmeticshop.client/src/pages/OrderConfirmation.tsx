import React from 'react';
import { useLocation } from 'react-router-dom';
import { CartItemResponseDto } from '../apiClient/models/cart-item-response-dto';

const OrderConfirmation: React.FC = () => {
  const location = useLocation();
  const order = location.state?.order;

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
    <div>
      <h1>Заказ успешно оформлен!</h1>
      <p>Ваш заказ №{order.id} успешно оформлен.</p>
      <p>Детали доставки будут отправлены на вашу электронную почту.</p>
      <div>
        <h2>Состав заказа:</h2>
        <ul>
          {order.orderItems.map((item: CartItemResponseDto) => (
            <li key={item.productId}>
              {item.productName} - {item.quantity} x {item.productPrice} = {item.quantity * item.productPrice}
            </li>
          ))}
        </ul>
        <p>Сумма заказа: {order.totalAmount}</p>
        <p>Количество товаров: {order.totalQuantity}</p>
      </div>
      <div>
        <h2>Метод доставки</h2>
        <p>{order.orderShippingMethod === 0 ? 'Почта' : order.orderShippingMethod === 1 ? 'Курьер' : 'Самовывоз'}</p>
      </div>
      <div>
        <h2>Метод оплаты</h2>
        <p>{order.orderPaymentMethod === 0 ? 'Переводом на карту' : order.orderPaymentMethod === 1 ? 'Наличными при получении' : 'Не указан'}</p>
      </div>
      <button onClick={() => (window.location.href = '/')}>На главную</button>
    </div>
  );
};

export default OrderConfirmation;