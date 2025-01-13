import React, { useEffect, useState } from 'react';
import { CartService } from '../apiClient/http-services/cart.service';
import { CartItemRequestDto, CartResponseDto } from '../apiClient/models';

const cartService = new CartService('/api'); // Базовый путь соответствует префиксу проксирования

const CartComponent: React.FC = () => {
  const [cart, setCart] = useState<CartResponseDto | null>(null);
  const [error, setError] = useState<string | null>(null);
  const abortController = new AbortController();
  const { signal } = abortController;

  useEffect(() => {
    const fetchCart = async () => {
      try {
        const cartData = await cartService.getCart(signal);
        setCart(cartData);
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
      abortController.abort();
    };
  }, []);

  const handleUpdateItem = async (productId: string, quantity: number) => {
    try {
      const item: CartItemRequestDto =  {productId, quantity};
      const updatedCart = await cartService.updateItemQuantity(item, signal);
      setCart(updatedCart);
    } catch (error) {
      console.error('Failed to add item to cart:', error);
      setError('Failed to add item to cart. Please try again later.');
    }
  };

  const handleRemoveItem = async (productId: string) => {
    try {
      await cartService.removeItemFromCart(productId, signal);
      const updatedCart = await cartService.getCart(signal);
      setCart(updatedCart);
    } catch (error) {
      console.error('Failed to remove item from cart:', error);
      setError('Failed to remove item from cart. Please try again later.');
    }
  };

  const handleClearCart = async () => {
    try {
      await cartService.clearCart(signal);
      setCart(null);
    } catch (error) {
      console.error('Failed to clear cart:', error);
      setError('Failed to clear cart. Please try again later.');
    }
  };

  return (
    <div>
      <h1>Shopping Cart</h1>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      {cart ? (
        <div>
          <ul>
            {cart.cartItems.map((item) => (
              <li key={item.productId}>
                {item.productName} - {item.quantity} x {item.productPrice} = {item.quantity * item.productPrice}
                <button onClick={() => handleRemoveItem(item.productId)}>Remove</button>
                <button onClick={() => handleUpdateItem(item.productId, item.quantity + 1 )}>+</button>
                <button onClick={() => handleUpdateItem(item.productId, item.quantity - 1 )}>-</button>
              </li>
            ))}
          </ul>
          <p>Total Amount: {cart.totalAmount}</p>
          <button onClick={handleClearCart}>Clear Cart</button>
        </div>
      ) : (
        <p>Cart is empty</p>
      )}
    </div>
  );
};

export default CartComponent;