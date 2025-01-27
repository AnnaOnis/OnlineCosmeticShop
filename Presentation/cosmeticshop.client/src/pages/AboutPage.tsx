import '../styles/AboutPage.css';

const AboutPage = () => {
  return (
    <div className="about-container">
      <section className="hero-section-about">
        <div className="hero-content-about">
          <h1 className="hero-title-about">О нашем бренде</h1>
          <p className="hero-subtitle-about">Создавая красоту с заботой о природе</p>
        </div>
      </section>

      <section className="mission-section">
        <div className="mission-content">
          <div className="mission-text">
            <h2>Наша миссия</h2>
            <p>
              Мы верим, что настоящая красота рождается из гармонии с природой. 
              Каждый наш продукт создается с любовью и вниманием к деталям, 
              чтобы вы могли чувствовать себя прекрасно без вреда для окружающей среды.
            </p>
          </div>
          <div className="mission-image">
            <img 
              src="/images/фон_1.jpg" 
              alt="Натуральные ингредиенты" 
              className="rounded-image"
            />
          </div>
        </div>
      </section>

      <section className="values-section">
        <h2 className="section-title">Наши ценности</h2>
        <div className="values-grid">
          <div className="value-card">
            <div className="value-icon">🌿</div>
            <h3>Натуральность</h3>
            <p>Только природные компоненты в составе наших продуктов</p>
          </div>
          <div className="value-card">
            <div className="value-icon">♻️</div>
            <h3>Экологичность</h3>
            <p>Биоразлагаемая упаковка и этичное производство</p>
          </div>
          <div className="value-card">
            <div className="value-icon">❤️</div>
            <h3>Забота</h3>
            <p>Продукты, безопасные для чувствительной кожи</p>
          </div>
        </div>
      </section>

      <section className="team-section">
        <h2 className="section-title">Наша команда</h2>
        <div className="team-grid">
          <div className="team-member">
            <img src="/images/основатель.jpg" alt="Основатель" />
            <h4>Анна Иванова</h4>
            <p>Основатель и главный химик</p>
          </div>
          <div className="team-member">
            <img src="/images/дизайнер.jpg" alt="Дизайнер" />
            <h4>Мария Петрова</h4>
            <p>Главный дизайнер упаковки</p>
          </div>
          <div className="team-member">
            <img src="/images/технолог.jpg" alt="Технолог" />
            <h4>Ольга Сидорова</h4>
            <p>Технолог производства</p>
          </div>
        </div>
      </section>
    </div>
  );
};

export default AboutPage;