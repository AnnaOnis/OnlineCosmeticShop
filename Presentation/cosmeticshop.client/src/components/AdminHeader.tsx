import { Link } from 'react-router-dom';
import '../styles/Header.css';
import { useAuth } from '../context/AuthContext';

const AdminHeader: React.FC = () => {
    const { logout } = useAuth();


  return (
    <header className="header">
      <nav className="nav-container container">
        <div className="logo">
          <span className="logo-icon">🌸</span>
          COSMETIC
        </div>

        <div className="nav-links">
          <Link to="/product-admin-table" className="nav-link">Товары</Link>
          <Link to="/order-admin-table" className="nav-link">Заказы</Link>
          <Link to="/review-admin-table" className="nav-link">Отзывы</Link>
          <Link to="/user-admin-table" className="nav-link">Пользователи</Link>
          <Link to="/admin/profile" className="nav-link">Профиль администратора</Link>
          <Link to="/login" className="nav-link" onClick={logout}>Выйти</Link>
        </div>
      </nav>
    </header>
  );
};

export default AdminHeader;