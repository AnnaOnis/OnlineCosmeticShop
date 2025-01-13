import React, { useState } from 'react';
import axios from 'axios';

interface Address {
  street: string;
  city: string;
  zip: string;
  country: string;
}

const Checkout: React.FC = () => {
  const [address, setAddress] = useState<Address>({
    street: '',
    city: '',
    zip: '',
    country: '',
  });
  const [paymentMethod, setPaymentMethod] = useState('credit-card');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await axios.post('/api/checkout', { address, paymentMethod });
      console.log(response.data);
      // Перенаправление на страницу подтверждения заказа
      window.location.href = '/order-confirmation';
    } catch (error) {
      console.error(error);
      alert('Ошибка при оформлении заказа');
    }
  };

  return (
    <div>
      <h1>Оформление заказа</h1>
      <form onSubmit={handleSubmit}>
        <h2>Адрес доставки</h2>
        <div>
          <label>Улица:</label>
          <input
            type="text"
            value={address.street}
            onChange={(e) => setAddress({ ...address, street: e.target.value })}
            required
          />
        </div>
        <div>
          <label>Город:</label>
          <input
            type="text"
            value={address.city}
            onChange={(e) => setAddress({ ...address, city: e.target.value })}
            required
          />
        </div>
        <div>
          <label>Индекс:</label>
          <input
            type="text"
            value={address.zip}
            onChange={(e) => setAddress({ ...address, zip: e.target.value })}
            required
          />
        </div>
        <div>
          <label>Страна:</label>
          <input
            type="text"
            value={address.country}
            onChange={(e) => setAddress({ ...address, country: e.target.value })}
            required
          />
        </div>
        <h2>Способ оплаты</h2>
        <div>
          <label>
            <input
              type="radio"
              value="credit-card"
              checked={paymentMethod === 'credit-card'}
              onChange={(e) => setPaymentMethod(e.target.value)}
            />
            Кредитная карта
          </label>
        </div>
        <div>
          <label>
            <input
              type="radio"
              value="paypal"
              checked={paymentMethod === 'paypal'}
              onChange={(e) => setPaymentMethod(e.target.value)}
            />
            PayPal
          </label>
        </div>
        <button type="submit">Оформить заказ</button>
      </form>
    </div>
  );
};

export default Checkout;