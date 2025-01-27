import React, { useEffect, useState } from 'react';
import { CartService } from '../apiClient/http-services/cart.service';
import { CartItemRequestDto} from '../apiClient/models';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { useCart } from '../context/CartContext';
import '../styles/Cart.css';

const cartService = new CartService('/api'); // Базовый путь соответствует префиксу проксирования

const CartComponent: React.FC = () => {
  const { cart, dispatch } = useCart();
  const [error, setError] = useState<string | null>(null);
  const [selectedItems, setSelectedItems] = useState<string[]>([]);
  const [selectAllText, setSelectAllText] = useState<string>('Выделить все');  
  const navigate = useNavigate();
  const { isAuthenticated} = useAuth();
  
  
  useEffect(() => {
    const fetchCart = async () => {
      if(!isAuthenticated){
        setError('Чтобы добавить товары в корзину ВОЙДИТЕ или ЗАРЕГИСТРИРУЙТЕСЬ!');
      }
      else{
        try {
          const cartData = await cartService.getCart(new AbortController().signal);
          dispatch({ type: 'SET_CART', payload: cartData });
          console.log(cartData);
        } catch (error) {
          if (error instanceof Error && error.name !== 'AbortError') {
            console.error('Failed to fetch cart:', error);
            setError('Failed to fetch cart. Please try again later.');
          }
        }        
      }
    };

    fetchCart();

    // Отмена запроса при размонтировании компонента
    return () => {
      new AbortController().abort();
    };
  }, []);

  const handleUpdateItem = async (productId: string, quantity: number) => {
    try {
      const item: CartItemRequestDto =  {productId, quantity};
      const updatedCart = await cartService.updateItemQuantity(item, new AbortController().signal);
      dispatch({ type: 'SET_CART', payload: updatedCart });
    } catch (error) {
      console.error('Failed to add item to cart:', error);
      setError('Failed to add item to cart. Please try again later.');
    }
  };

  const handleRemoveItem = async (productId: string) => {
    try {
      await cartService.removeItemFromCart(productId, new AbortController().signal);
      const updatedCart = await cartService.getCart(new AbortController().signal);
      dispatch({ type: 'SET_CART', payload: updatedCart });
    } catch (error) {
      console.error('Failed to remove item from cart:', error);
      setError('Failed to remove item from cart. Please try again later.');
    }
  };

  const handleClearCart = async () => {
    try {
      await cartService.clearCart(new AbortController().signal);
      dispatch({ type: 'SET_CART', payload: null });
      setSelectedItems([]); // Снимаем выделение всех товаров при очистке корзины
      setSelectAllText('Выделить все'); // Возвращаем текст кнопки к исходному
    } catch (error) {
      console.error('Failed to clear cart:', error);
      setError('Failed to clear cart. Please try again later.');
    }
  };

  const toggleSelectItem = (productId: string) => {
    setSelectedItems(prev => prev.includes(productId)
      ? prev.filter(id => id !== productId)
      : [...prev, productId]);
  };

  const toggleSelectAll = () => {
    if (cart?.cartItems) {
      if (selectedItems.length === cart.cartItems.length) {
        setSelectedItems([]);
        setSelectAllText('Выделить все');
      } else {
        setSelectedItems(cart.cartItems.map(item => item.productId));
        setSelectAllText('Отменить выделение');
      }
    } else {
      setSelectedItems([]);
      setSelectAllText('Выделить все');
    }
  };

  const filteredCartItems = cart?.cartItems.filter(item => selectedItems.includes(item.productId)) ?? [];
  const isDisabled = filteredCartItems.length === 0;

  const handleCheckout = () => {
    if (!isDisabled) {
      navigate("/checkout", { state: { selectedItems: filteredCartItems } });
    }
  };

  return (
    <div className="cart-container">
      <div className="cart-header">
        <h1 className="cart-title">Корзина</h1>
        {cart && (
          <button 
            className="select-all-button"
            onClick={toggleSelectAll}
          >
            {selectAllText}
          </button>
        )}
      </div>

      {error && <p className="error-message">{error}</p>}

      {cart ? (
        <>
          <div className="cart-items-list">
            {cart.cartItems
              .slice()
              .sort((a, b) => a.productName.localeCompare(b.productName))
              .map((item) => (
                <div className="cart-item" key={item.productId}>
                  <input
                    type="checkbox"
                    checked={selectedItems.includes(item.productId)}
                    onChange={() => toggleSelectItem(item.productId)}
                    className="item-checkbox"
                  />
                  
                  {/* <div className="product-image-container">
                    <img 
                      src={item.imageUrl || '/placeholder-image.jpg'} 
                      alt={item.productName} 
                      className="product-image"
                    />
                  </div> */}

                  <div className="item-details">
                    <h3 className="product-name">{item.productName}</h3>
                    <p className="product-price">{item.productPrice} руб.</p>

                    <div className="quantity-controls">
                      <button 
                        className="quantity-button"
                        onClick={() => handleUpdateItem(item.productId, item.quantity - 1)}
                        disabled={item.quantity <= 1}
                      >-</button>
                      
                      <span>{item.quantity}</span>
                      
                      <button 
                        className="quantity-button"
                        onClick={() => handleUpdateItem(item.productId, item.quantity + 1)}
                      >+</button>
                    </div>

                    <div className="item-actions">
                      <button 
                        className="favorite-button"
                        onClick={() => {/* Добавить логику для избранного */}}
                      >
                        В избранное
                      </button>
                      <button 
                        className="remove-button"
                        onClick={() => handleRemoveItem(item.productId)}
                      >
                        Удалить
                      </button>
                    </div>
                  </div>
                </div>
              ))}
          </div>

          <div className="checkout-section">
            <div className="cart-actions">
              <button 
                className="clear-cart-button"
                onClick={handleClearCart}
                disabled={!cart || cart.cartItems.length === 0}
              >
                Очистить корзину
              </button>
            </div>
            
            <div className="total-block">
              <p className="total-amount">
                Итого: {filteredCartItems.reduce((total, item) => 
                  total + item.quantity * item.productPrice, 0)} руб.
              </p>
              <button 
                className="checkout-button"
                onClick={handleCheckout} 
                disabled={isDisabled}
              >
                Оформить заказ
              </button>
            </div>
          </div>
        </>
      ) : (
        !error && <p className="empty-cart">Корзина пуста</p>
      )}
    </div>
  );
};
export default CartComponent;