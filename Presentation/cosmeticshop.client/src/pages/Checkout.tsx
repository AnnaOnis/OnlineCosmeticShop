import React, { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import { OrderService } from '../apiClient/http-services/order.service';
import { OrderCreateRequestDto, CartItemRequestDto, ShippingMethod, PaymentMethod, CartItemResponseDto } from '../apiClient/models';
import { useNavigate, useLocation } from 'react-router-dom';
import { useCart } from '../context/CartContext';
import { CartService } from '../apiClient/http-services/cart.service';
import '../styles/Checkout.css'

const orderService = new OrderService('/api'); // Базовый путь соответствует префиксу проксирования

const CheckoutComponent: React.FC = () => {
  const { isAuthenticated, id: customerId } = useAuth();
  const { dispatch } = useCart();
  const navigate = useNavigate();
  const location = useLocation();
  const [cartItems, setCartItems] = useState<CartItemResponseDto[]>([]);
  const [totalAmount, setTotalAmount] = useState<number>(0);
  const [totalQuantity, setTotalQuantity] = useState<number>(0);
  const [shippingMethod, setShippingMethod] = useState<ShippingMethod>(ShippingMethod.NUMBER_0);
  const [paymentMethod, setPaymentMethod] = useState<PaymentMethod>(PaymentMethod.NUMBER_0);
  const [error, setError] = useState<string | null>(null);
  const cartService = new CartService('/api');

  useEffect(() => {
    if (!isAuthenticated) {
      navigate('/login');
    } else {
      const selectedItems = location.state?.selectedItems as CartItemResponseDto[];
      if (selectedItems) {
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
      dispatch({ type: 'SET_CART', payload: null });
      await cartService.clearCart(new AbortController().signal);
      navigate('/order-confirmation', { state: { order: createdOrder } });
    } catch (error) {
      console.error('Ошибка при создании заказа:', error);
      setError('Ошибка при создании заказа. Попробуйте снова.');
    }
  };

  return (
    <div className="checkout-container">
      <h1 className="checkout-title">Оформление заказа</h1>
      
      {error && <div className="error-message">{error}</div>}
  
      <div className="checkout-section">
        <h2 className="section-title">Выбранные товары</h2>
        <div className="checkout-items-list">
          {cartItems.map((item) => (
            <div key={item.productId} className="checkout-item">
              <span className="checkout-item-name">{item.productName}</span>
              <span className="checkout-item-quantity">
                <span>{item.quantity} </span> 
                <span>x</span> 
                <span>{item.productPrice}</span>  
                <span>₽</span>
              </span>
              <span className="checkout-item-total">{(item.quantity * item.productPrice).toLocaleString()} ₽</span>
            </div>
          ))}
        </div>
        
        <div className="totals-box">
          <div className="total-row">
            <span>Количество товаров: </span>
            <span>{totalQuantity}</span>
          </div>
          <div className="total-row main-total">
            <span>Сумма: </span>
            <span>{totalAmount.toLocaleString()} ₽</span>
          </div>
        </div>
      </div>
  
      <div className="checkout-section">
        <h2 className="section-title">Способ доставки</h2>
        <select 
          className="method-select"
          value={shippingMethod} 
          onChange={(e) => setShippingMethod(Number(e.target.value) as ShippingMethod)}
        >
          <option value={ShippingMethod.NUMBER_0}>Почта России</option>
          <option value={ShippingMethod.NUMBER_1}>Курьерская доставка</option>
          <option value={ShippingMethod.NUMBER_2}>Самовывоз из магазина</option>
        </select>
      </div>
  
      <div className="checkout-section">
        <h2 className="section-title">Способ оплаты</h2>
        <select 
          className="method-select"
          value={paymentMethod} 
          onChange={(e) => setPaymentMethod(Number(e.target.value) as PaymentMethod)}
        >
          <option value={PaymentMethod.NUMBER_0}>Банковской картой онлайн</option>
          <option value={PaymentMethod.NUMBER_1}>Наличными при получении</option>
        </select>
      </div>
  
      <button 
        className="confirm-button"
        onClick={handleCreateOrder}
      >
        Подтвердить заказ
      </button>
    </div>
  );
};

export default CheckoutComponent;