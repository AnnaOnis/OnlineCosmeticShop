import React, { createContext, useContext, useReducer, useEffect } from 'react';
import { CartService } from '../apiClient/http-services/cart.service';
import { CartResponseDto } from '../apiClient/models';
import { useAuth } from '../context/AuthContext';

interface CartState {
  cart: CartResponseDto | null;
  cartItemCount: number;
  dispatch: React.Dispatch<{ type: string; payload?: any }>;
}

const CartContext = createContext<CartState | undefined>(undefined);

export const useCart = () => {
  const context = useContext(CartContext);
  if (!context) {
    throw new Error('useCart must be used within a CartProvider');
  }
  return context;
};

const cartReducer = (state: CartState, action: { type: string; payload?: any }): CartState => {
  switch (action.type) {
    case 'SET_CART':
      return { ...state, cart: action.payload, cartItemCount: (action.payload?.cartItems?.length || 0) };
    default:
      return state;
  }
};

export const CartProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [cartState, setCart] = useReducer(cartReducer, { cart: null, cartItemCount: 0, dispatch: () => {} });
  const { isAuthenticated, role} = useAuth();
  const cartService = new CartService('/api');

  useEffect(() => {
    const fetchCart = async () => {
        if (!isAuthenticated || role != null) {
          console.log('User is not authenticated or is an admin, skipping cart fetch.');
          return;
        }
  
        try {
          const cartData = await cartService.getCart(new AbortController().signal);
          setCart({ type: 'SET_CART', payload: cartData });
        } catch (error) {
          console.error('Failed to fetch cart:', error);
        }
      };

    fetchCart();

    // Отмена запроса при размонтировании компонента
    return () => {
      setCart({ type: 'SET_CART', payload: null });
    };
  }, [isAuthenticated, role]);

  return (
    <CartContext.Provider value={{ ...cartState, dispatch: setCart }}>
      {children}
    </CartContext.Provider>
  );
};