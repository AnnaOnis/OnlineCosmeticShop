import { Link } from 'react-router-dom';
import '../styles/Header.css';
import { useAuth } from '../context/AuthContext';
import { useCart } from '../context/CartContext';

const Header = () => {
  const { isAuthenticated, role, logout } = useAuth();
  const { cartItemCount } = useCart();

  return (
    <header className="header">
      <nav className="nav-container container">
        <Link to="/" className="logo">
          <span className="logo-icon">🌸</span>
          COSMETIC
        </Link>

        <div className="nav-links">
          <Link to="/catalog" className="nav-link">Каталог</Link>
          <Link to="/about" className="nav-link">О нас</Link>
          
          {isAuthenticated ? (
            <>
              {role != null ? (
                <>
                  <Link to="/admin/profile" className="nav-link">Админ панель</Link>
                </>
              ) : (
                <>
                  <Link to="/profile" className="nav-link">Профиль</Link>
                </>
              )}
              <Link to="/login" className="nav-link" onClick={logout}>Выйти</Link>
            </>
          ) : (
            <>
                <Link to="/login" className="nav-link">Вход</Link>
                <Link to="/register" className="nav-link">Регистрация</Link>            
            </>
          )}
          
          {!role && (          
            <>
              <Link to="/cart" className="cart-link">
                <i className="fa-solid fa-cart-shopping"></i>
                <span className="cart-counter">{cartItemCount}</span> {/* Динамическое значение из контекста */}
              </Link>            
            </>
          )}

        </div>
      </nav>
    </header>
  );
};

export default Header;