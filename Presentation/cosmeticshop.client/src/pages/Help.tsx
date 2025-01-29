import React from 'react';
import '../styles/Help.css';

const Help: React.FC = () => {
  return (
    <div className="help-container">
      <div className="help-content">
        <h1 className="page-title">Помощь</h1>
        
        <section className="help-section">
          <h2 className="section-title">Часто задаваемые вопросы</h2>
          
          <div className="faq-item">
            <h3 className="faq-question">Как оформить заказ?</h3>
            <p className="faq-answer">
              Для оформления заказа перейдите в раздел "Каталог товаров", выберите нужные товары 
              и добавьте их в корзину. Затем перейдите в корзину и нажмите кнопку "Оформить заказ". 
              Заполните форму доставки и выберите способ оплаты.
            </p>
          </div>

          <div className="faq-item">
            <h3 className="faq-question">Как отменить заказ?</h3>
            <p className="faq-answer">
              Для отмены заказа свяжитесь с нашей службой поддержки по телефону 
              <a href="tel:+74951234567" className="contact-link"> +7 (495) 123-45-67</a> или 
              отправьте сообщение на <a href="mailto:info@cosmeticshop.ru" className="contact-link">info@cosmeticshop.ru</a>.
            </p>
          </div>
        </section>

        <section className="payment-section">
          <h2 className="section-title">Оплата и доставка</h2>

          <div className="payment-section-grid">
            <div className="info-card">
              <h3>Способы оплаты</h3>
              <ul className="info-list">
                <li>Банковской картой онлайн</li>
                <li>Наложенный платеж</li>
                <li>Электронные кошельки</li>
              </ul>
            </div>

            <div className="info-card">
              <h3>Сроки доставки</h3>
              <ul className="info-list">
                <li>Москва — 1-2 дня</li>
                <li>Регионы России — 3-7 дней</li>
                <li>Самовывоз из нашего магазина</li>
              </ul>
            </div>            
          </div>

        </section>
      </div>
    </div>
  );
};

export default Help;