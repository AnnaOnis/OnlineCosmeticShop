import React, { useEffect, useState } from 'react';
import { CartService } from '../apiClient/http-services/cart.service';
import { FavoriteService } from '../apiClient/http-services/favorite.service';
import { CartItemRequestDto, FavoriteRequestDto, ProductResponseDto} from '../apiClient/models';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { useCart } from '../context/CartContext';
import '../styles/Cart.css';


const cartService = new CartService('/api'); // Базовый путь соответствует префиксу проксирования
const favoriteService = new FavoriteService('/api');

const CartComponent: React.FC = () => {
  const { cart, dispatch } = useCart();
  const [error, setError] = useState<string | null>(null);
  const [selectedItems, setSelectedItems] = useState<string[]>([]);
  const [selectAllText, setSelectAllText] = useState<string>('Выделить все');  
  const [favoriteProducts, setFavoriteProducts] = useState<Set<string>>(new Set());
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const navigate = useNavigate();
  const { isAuthenticated, customerId} = useAuth();
  
  
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

    const fetchFavorites = async () => {
      try {
        if(customerId){
          const response = await favoriteService.getAllFavoritesByCustomerId(customerId, new AbortController().signal);
          setFavoriteProducts(new Set(response.map((product: ProductResponseDto) => product.id)));  
          console.log(response);        
        }
      } catch (error) {
        console.error(error);
      }
    };

    fetchFavorites();

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

  const handleAddOrRemoveProductToFavorites = async (productId: string) => {

    if (!customerId) {
      setErrorMessage('Чтобы добавить товары в избранное ВОЙДИТЕ или ЗАРЕГИСТРИРУЙТЕСЬ!');
      setTimeout(() => setErrorMessage(null), 2000);
      return;
    }

    try {
      const request: FavoriteRequestDto = {
        productId: productId,
        customerId: customerId
      };

      if (favoriteProducts.has(productId)) {
        // Удаляем товар из избранного
        await favoriteService.removeFromFavorites(productId, customerId, new AbortController().signal);
        setFavoriteProducts(prev => new Set([...prev].filter(id => id !== productId)));
      } else {
        // Добавляем товар в избранное
        await favoriteService.addToFavorites(request, new AbortController().signal);
        setFavoriteProducts(prev => new Set([...prev, productId]));
      }
    } catch (error) {
      console.error("Ошибка при добавлении/удалении товара из избранного", error);
      setErrorMessage('Ошибка при добавлении/удалении товара из избранного');
      setTimeout(() => setErrorMessage(null), 2000);
    }
  };

  return (
    <div className="cart-container">
      <div className="cart-header">
        <h1 className="cart-title">Корзина</h1>
        {cart && (
          <button 
            className="btn btn-outline"
            onClick={toggleSelectAll}
          >
            {selectAllText}
          </button>
        )}
      </div>

      {error && <p className="error-message">{error}</p>}
      {errorMessage && <div className="message error">{errorMessage}</div>}

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
                  
                  <div className="product-image-container">
                    <img 
                      src={item.productImageUrl || '/images/фото пока нет.jpg'} 
                      alt={item.productName} 
                      className="product-image"
                    />
                  </div>

                  <div className="item-details">
                    <div className='cart-item-info'>
                      <h3 className="cart-product-name">{item.productName}</h3>
                      <p className="cart-product-price">{item.productPrice} руб.</p>                      
                    </div>
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
                      {(() => {
                        const isFavorite = favoriteProducts.has(item.productId);
                        return (
                          <button 
                            className={`btn btn-outline ${isFavorite ? 'cart-favorite-btn' : ''}`}
                            onClick={() => handleAddOrRemoveProductToFavorites(item.productId)}
                            >
                            <i className="fas fa-heart"></i>
                          </button>
                                );
                      })()}
                      <button 
                        className="btn btn-outline"
                        onClick={() => handleRemoveItem(item.productId)}
                      >
                        <i className="fas fa-trash-alt"></i>
                      </button>
                    </div>
                  </div>
                </div>
              ))}
          </div>

          <div className="cart-checkout-section">
            <div className="cart-actions">
              <button 
                className="btn btn-outline"
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
                className="btn btn-primary"
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