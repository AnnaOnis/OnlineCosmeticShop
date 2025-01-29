import '../styles/Footer.css';
import { Link } from 'react-router-dom';

const Footer = () => {
  return (
    <footer className="footer">
      <div className="footer-container">
        {/* Логотип и описание */}
        <div className="footer-section">
          <Link to="/" className="footer-logo">
            <span className="logo-icon">🌸</span>
            COSMETIC
          </Link>
          <p className="footer-description">
            Красота досупная каждому!
          </p>
        </div>

        {/* Навигация */}
        <div className="footer-section">
          <ul className="footer-links">
            <li><Link to="/">Главная</Link></li>
            <li><Link to="/catalog">Каталог</Link></li>
            <li><Link to="/about">О магазине</Link></li>            
          </ul>
        </div>

        {/* Помощь */}
        <div className="footer-section">
          <ul className="footer-links">
            <li><Link to="/help">Помощь</Link></li>
            <li><Link to="/contacts">Контакты</Link></li>
            <li><Link to="/help">Обратная связь</Link></li>
          </ul>
        </div>

        {/* Подписка */}
        <div className="footer-section">
          <h3 className="footer-title">Подписка</h3>
          <form className="subscribe-form">
            <input 
              type="email" 
              placeholder="Ваш email" 
              className="subscribe-input"
            />
            <button type="submit" className="subscribe-btn">
              Подписаться
            </button>
          </form>
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

export default Footer;