import '../styles/Home.css';

const Home = () => {
  return (
    <main>
      <section className="hero-section-home">
        <div className="container">
          <div className="hero-content-home">
            <h1 className="hero-title-home">Только качественная и натуральная косметика по доступным ценам</h1>
            <p className="hero-subtitle-home">Самые популярные и любимые бренды</p>
            <button className="btn btn-primary">Каталог</button>
          </div>
        </div>
      </section>

      <section className="benefits-section">
        <div className="container">
          <div className="benefits-grid">
            <div className="benefit-card">
              <i className="fas fa-boxes fa-2x"></i> {/* Иконка для широкого ассортимента */}
              <h3>Широкий ассортимент товаров</h3>
              <p>Большой выбор косметики от известных брендов</p>
            </div>
            <div className="benefit-card">
              <i className="fas fa-check-circle fa-2x"></i> {/* Иконка для гарантии качества */}
              <h3>Гарантия качества</h3>
              <p>Все товары проходят строгий контроль качества</p>
            </div>
            <div className="benefit-card">
              <i className="fas fa-gift fa-2x"></i> {/* Иконка для акций и скидок */}
              <h3>Частые акции и скидки</h3>
              <p>Уникальные предложения и скидки для наших клиентов</p>
            </div>
            <div className="benefit-card">
              <i className="fas fa-desktop fa-2x"></i> {/* Иконка для удобного интерфейса сайта */}
              <h3>Удобный интерфейс сайта</h3>
              <p>Легко находить нужные товары и оформлять заказы</p>
            </div>
          </div>
        </div>
      </section>
    </main>
  );
};

export default Home;