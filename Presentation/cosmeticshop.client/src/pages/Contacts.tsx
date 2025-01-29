import React from 'react';
import '../styles/Contacts.css';

const Contacts: React.FC = () => {
  return (
    <div className="contacts-container">
      <div className="contacts-content">
        <h1 className="page-title">Контакты</h1>
        
        <div className="contact-info">
          <div className="info-card">
            <h2 className="info-title">Адрес</h2>
            <p>г. Москва, ул. Ленина, д. 1</p>
            <p>Вход со стороны парковки</p>
          </div>

          <div className="info-card">
            <h2 className="info-title">Телефон</h2>
            <a href="tel:+74951234567" className="contact-link">+7 (495) 123-45-67</a>
            <p>Ежедневно с 9:00 до 21:00</p>
          </div>

          <div className="info-card">
            <h2 className="info-title">Email</h2>
            <a href="mailto:info@cosmeticshop.ru" className="contact-link">info@cosmeticshop.ru</a>
            <p>Ответим в течение 24 часов</p>
          </div>
        </div>

        <section className="contact-form-section">
          <h2 className="section-title">Форма обратной связи</h2>
          <form className="contact-form">
            <div className="form-group">
              <label>Имя</label>
              <input 
                type="text" 
                className="form-input"
                required 
              />
            </div>
            
            <div className="form-group">
              <label>Email</label>
              <input 
                type="email" 
                className="form-input"
                required 
              />
            </div>
            
            <div className="form-group">
              <label>Сообщение</label>
              <textarea 
                className="form-textarea"
                required 
              />
            </div>
            
            <button type="submit" className="submit-btn">
              Отправить сообщение
            </button>
          </form>
        </section>
      </div>
    </div>
  );
};

export default Contacts;