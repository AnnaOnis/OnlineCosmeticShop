import '../styles/Footer.css';
import { Link } from 'react-router-dom';

const AdminFooter = () => {
  return (
    <footer className="footer">
      <div className="footer-container">
        {/* Логотип и описание */}
        <div className="footer-section">
          <div className="footer-logo">
            <span className="logo-icon">🌸</span>
            COSMETIC
          </div>
          <p className="footer-description">
            Красота досупная каждому!
          </p>
        </div>

        <div className="footer-section">
          <ul className="footer-links">
            <li><Link to="/admin/products" className="nav-link">Товары</Link></li>
            <li><Link to="/admin/orders" className="nav-link">Заказы</Link></li>         
          </ul>
        </div>

        <div className="footer-section">
          <ul className="footer-links">
            <li><Link to="/admin/reviews" className="nav-link">Отзывы</Link></li>
            <li><Link to="/admin/users" className="nav-link">Пользователи</Link></li>
          </ul>
        </div>

      <div className="footer-section">
          <ul className="footer-links">
            <li><Link to="/admin/profile" className="nav-link">Профиль администратора</Link></li>
          </ul>
        </div>
      </div>

      {/* Копирайт */}
      <div className="footer-bottom">
        <p>© 2024 COSMETIC. Все права защищены</p>
        <div className="legal-links">
          <Link to="/privacy">Политика конфиденциальности</Link>
          <Link to="/terms">Условия использования</Link>
        </div>
      </div>
    </footer>
  );
};

export default AdminFooter;