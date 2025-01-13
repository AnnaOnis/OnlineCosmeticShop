import React from 'react';

const OrderConfirmation: React.FC = () => {
  return (
    <div>
      <h1>Заказ успешно оформлен!</h1>
      <p>Ваш заказ №12345 успешно оформлен.</p>
      <p>Детали доставки будут отправлены на вашу электронную почту.</p>
      <button onClick={() => (window.location.href = '/')}>На главную</button>
    </div>
  );
};

export default OrderConfirmation;