import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Navbar: React.FC = () => {
  const { isAuthenticated, logout } = useAuth();

  const handleLogout = () => {
    logout();
  };

  return (
    <nav style={{ marginBottom: '2rem' }}>
      <ul style={{ display: 'flex', gap: '1rem', listStyle: 'none', padding: 0 }}>
        <li><Link to="/">Главная</Link></li>
        <li><Link to="/catalog">Каталог</Link></li>
        <li><Link to="/cart">Корзина</Link></li>
        {isAuthenticated ? (
          <>
            <li><Link to="/profile">Профиль</Link></li>
            <li><Link to="/login" onClick={handleLogout}>Выход</Link></li>
          </>
        ) : (
          <>
            <li><Link to="/login">Вход</Link></li>
            <li><Link to="/register">Регистрация</Link></li>
          </>
        )}
        <li><Link to="/contacts">Контакты</Link></li>
        <li><Link to="/help">Помощь</Link></li>
      </ul>
    </nav>
  );
};

export default Navbar;