import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import AppRoutes from './AppRoutes';
//import './index.css'
import './styles/global.css';
import { AuthProvider } from './context/AuthContext';
import { CartProvider } from './context/CartContext';
import '@fortawesome/fontawesome-free/css/all.min.css';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <AuthProvider>
      <CartProvider>
        <AppRoutes />
      </CartProvider>      
    </AuthProvider>  
  </StrictMode>,
)
