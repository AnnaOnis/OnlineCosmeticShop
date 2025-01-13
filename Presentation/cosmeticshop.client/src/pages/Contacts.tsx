import React from 'react';

const Contacts: React.FC = () => {
  return (
    <div>
      <h1>Контакты</h1>
      <p>Адрес: г. Москва, ул. Ленина, д. 1</p>
      <p>Телефон: +7 (495) 123-45-67</p>
      <p>Email: info@cosmeticshop.ru</p>
      <h2>Форма обратной связи</h2>
      <form>
        <div>
          <label>Имя:</label>
          <input type="text" required />
        </div>
        <div>
          <label>Email:</label>
          <input type="email" required />
        </div>
        <div>
          <label>Сообщение:</label>
          <textarea required />
        </div>
        <button type="submit">Отправить</button>
      </form>
    </div>
  );
};

export default Contacts;