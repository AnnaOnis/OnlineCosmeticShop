import React, { useEffect, useState } from 'react';
import { CartService } from '../apiClient/http-services/cart.service';
import { CartItemRequestDto, CartResponseDto } from '../apiClient/models';
import { Link } from 'react-router-dom';

const cartService = new CartService('/api'); // Базовый путь соответствует префиксу проксирования

const CartComponent: React.FC = () => {
  const [cart, setCart] = useState<CartResponseDto | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [selectedItems, setSelectedItems] = useState<string[]>([]);
  
  useEffect(() => {
    const fetchCart = async () => {
      try {
        const cartData = await cartService.getCart(new AbortController().signal);
        setCart(cartData);
        console.log(cartData);
      } catch (error) {
        if (error instanceof Error && error.name !== 'AbortError') {
          console.error('Failed to fetch cart:', error);
          setError('Failed to fetch cart. Please try again later.');
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
      setCart(updatedCart);
    } catch (error) {
      console.error('Failed to add item to cart:', error);
      setError('Failed to add item to cart. Please try again later.');
    }
  };

  const handleRemoveItem = async (productId: string) => {
    try {
      await cartService.removeItemFromCart(productId, new AbortController().signal);
      const updatedCart = await cartService.getCart(new AbortController().signal);
      setCart(updatedCart);
    } catch (error) {
      console.error('Failed to remove item from cart:', error);
      setError('Failed to remove item from cart. Please try again later.');
    }
  };

  const handleClearCart = async () => {
    try {
      await cartService.clearCart(new AbortController().signal);
      setCart(null);
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

  const filteredCartItems = cart?.cartItems.filter(item => selectedItems.includes(item.productId)) ?? [];

  return (
    <div>
      <h1>Shopping Cart</h1>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      {cart ? (
        <div>
          <ul>
            {cart.cartItems.map((item) => (
              <li key={item.productId}>
                <input
                  type="checkbox"
                  checked={selectedItems.includes(item.productId)}
                  onChange={() => toggleSelectItem(item.productId)}
                />
                {item.productName} - {item.quantity} x {item.productPrice} = {item.quantity * item.productPrice}
                <button onClick={() => handleRemoveItem(item.productId)}>Remove</button>
                <button onClick={() => handleUpdateItem(item.productId, item.quantity + 1)}>+</button>
                <button onClick={() => handleUpdateItem(item.productId, item.quantity - 1)}>-</button>
              </li>
            ))}
          </ul>
          <p>Total Amount: {filteredCartItems.reduce((total, item) => total + item.quantity * item.productPrice, 0)}</p>
          <button onClick={handleClearCart}>Clear Cart</button>
          <div>
            <Link to="/checkout" state={{ selectedItems: filteredCartItems }}>
              Перейти к оформлению заказа
            </Link>
          </div>
        </div>
      ) : (
        <p>Cart is empty</p>
      )}
    </div>
  );
};

export default CartComponent;