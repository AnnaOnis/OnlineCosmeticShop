import React, { useState, useEffect } from 'react';
import { useAuth } from '../AuthContext';
import { OrderService } from '../apiClient/http-services/order.service';
import { OrderCreateRequestDto, CartItemRequestDto, ShippingMethod, PaymentMethod, CartItemResponseDto } from '../apiClient/models';
import { useNavigate, useLocation } from 'react-router-dom';

const orderService = new OrderService('/api'); // Базовый путь соответствует префиксу проксирования

const CheckoutComponent: React.FC = () => {
  const { isAuthenticated, customerId } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const [cartItems, setCartItems] = useState<CartItemResponseDto[]>([]);
  const [totalAmount, setTotalAmount] = useState<number>(0);
  const [totalQuantity, setTotalQuantity] = useState<number>(0);
  const [shippingMethod, setShippingMethod] = useState<ShippingMethod>(ShippingMethod.NUMBER_0);
  const [paymentMethod, setPaymentMethod] = useState<PaymentMethod>(PaymentMethod.NUMBER_0);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!isAuthenticated) {
      navigate('/login');
    } else {
      console.log("customerId:");
      console.log(customerId);
      const selectedItems = location.state?.selectedItems as CartItemResponseDto[];
      if (selectedItems) {
        console.log("selectedItems:");
        console.log(selectedItems);
        setCartItems(selectedItems);
        const totalAmount = selectedItems.reduce((total, item) => total + item.quantity * item.productPrice, 0);
        const totalQuantity = selectedItems.length;
        setTotalAmount(totalAmount);
        setTotalQuantity(totalQuantity);
      }
    }

    return () => {
      new AbortController().abort();
    };
  }, [isAuthenticated, location.state, navigate]);

  const handleCreateOrder = async () => {
    if (!customerId) {
      setError('Пользователь не авторизован');
      return;
    }
    const orderItems: CartItemRequestDto[] = cartItems.map(item => ({
      productId: item.productId,
      quantity: item.quantity,
    }));

    const orderRequest: OrderCreateRequestDto = {
      customerId: customerId,
      totalQuantity: totalQuantity, 
      totalAmount: totalAmount,
      orderShippingMethod: shippingMethod,
      orderPaymentMethod: paymentMethod,
      cartItems: orderItems,
    };
    console.log("orderRequest:");
    console.log(orderRequest);

      // Проверка данных перед отправкой
    if (!orderRequest.cartItems || orderRequest.cartItems.length === 0) {
      setError('Нет выбранных товаров');
      return;
    }

    for (const item of orderRequest.cartItems) {
      if (!item.productId || item.quantity <= 0) {
        setError('Некорректные данные о товаре');
        return;
      }
    }

    try {
      const createdOrder = await orderService.createOrderAsync(orderRequest, new AbortController().signal);
      setError(null);
      console.log("createdOrder:");
      console.log(createdOrder);
      alert('Заказ успешно создан!');
      navigate('/order-confirmation', { state: { order: createdOrder } });
    } catch (error) {
      console.error('Ошибка при создании заказа:', error);
      setError('Ошибка при создании заказа. Попробуйте снова.');
    }
  };

  return (
    <div>
      <h1>Оформление заказа</h1>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      <div>
        <h2>Выбранные товары</h2>
        <ul>
          {cartItems.map((item) => (
            <li key={item.productId}>
              {item.productName} - {item.quantity} x {item.productPrice} = {item.quantity * item.productPrice}
            </li>
          ))}
        </ul>
        <p>Общая сумма: {totalAmount}</p>
        <p>Общее количество: {totalQuantity}</p>
      </div>
      <div>
        <h2>Метод доставки</h2>
        <select value={shippingMethod} onChange={(e) => setShippingMethod(Number(e.target.value) as ShippingMethod)}>
          <option value={ShippingMethod.NUMBER_0}>Почта</option>
          <option value={ShippingMethod.NUMBER_1}>Курьер</option>
          <option value={ShippingMethod.NUMBER_2}>Самовывоз</option>
        </select>
      </div>
      <div>
        <h2>Метод оплаты</h2>
        <select value={paymentMethod} onChange={(e) => setPaymentMethod(Number(e.target.value) as PaymentMethod)}>
          <option value={PaymentMethod.NUMBER_0}>Перевод на карту</option>
          <option value={PaymentMethod.NUMBER_1}>Наличными при получении</option>
        </select>
      </div>
      <button onClick={handleCreateOrder}>Создать заказ</button>
    </div>
  );
};

export default CheckoutComponent;