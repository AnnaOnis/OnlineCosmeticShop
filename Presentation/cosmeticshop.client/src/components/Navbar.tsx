import React from 'react';
import { Link } from 'react-router-dom';

const Navbar: React.FC = () => {
  return (
    <nav style={{ marginBottom: '2rem' }}>
      <ul style={{ display: 'flex', gap: '1rem', listStyle: 'none', padding: 0 }}>
        <li><Link to="/">Главная</Link></li>
        <li><Link to="/catalog">Каталог</Link></li>
        <li><Link to="/cart">Корзина</Link></li>
        <li><Link to="/login">Вход</Link></li>
        <li><Link to="/register">Регистрация</Link></li>
        <li><Link to="/profile">Профиль</Link></li>
        <li><Link to="/contacts">Контакты</Link></li>
        <li><Link to="/help">Помощь</Link></li>
      </ul>
    </nav>
  );
};

export default Navbar;