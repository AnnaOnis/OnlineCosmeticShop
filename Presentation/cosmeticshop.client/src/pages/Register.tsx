import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { CustomerRegisterRequestDto } from '../apiClient/models/customer-register-request-dto';
import InputMask from 'react-input-mask';
import '../styles/Auth.css';

const Register: React.FC = () => {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [street, setStreet] = useState('');
  const [house, setHouse] = useState('');
  const [apartment, setApartment] = useState('');
  const [city, setCity] = useState('');
  const [zipCode, setZipCode] = useState('');
  const [showSuccessMessage, setShowSuccessMessage] = useState(false);
  const { register } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (password !== confirmPassword) {
      alert('Пароли не совпадают');
      return;
    }
    const registerRequest: CustomerRegisterRequestDto = {
      firstName,
      lastName,
      email,
      phoneNumber,
      shippingAddress: `${city}, ${street}, д. ${house}, кв. ${apartment}, индекс: ${zipCode}`,
      password,
      confirmedPassword: confirmPassword,
    };
    try {
      await register(registerRequest);
      setShowSuccessMessage(true);
      setTimeout(() => navigate('/login'), 5000);
    } catch (error) {
      console.error(error);
      alert('Ошибка при регистрации. Попробуйте снова.');
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-card">
        {showSuccessMessage ? (
          <div className="success-message">
            <p>Регистрация успешно завершена!</p>
            <p>Перенаправление на страницу входа...</p>
          </div>
        ) : (
          <>
            <h1 className="auth-title">Регистрация</h1>
            <form className="auth-form" onSubmit={handleSubmit}>
              <div className="form-group">
                <label className="form-label">Имя</label>
                <input
                  type="text"
                  className="form-input"
                  value={firstName}
                  onChange={(e) => setFirstName(e.target.value)}
                  required
                />
              </div>

              <div className="form-group">
                <label className="form-label">Фамилия</label>
                <input
                  type="text"
                  className="form-input"
                  value={lastName}
                  onChange={(e) => setLastName(e.target.value)}
                  required
                />
              </div>

              <div className="form-group">
                <label className="form-label">Email</label>
                <input
                  type="email"
                  className="form-input"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                />
              </div>

              <div className="form-group">
                <label className="form-label">Телефон</label>
                <InputMask
                  mask="+7(999)-999-99-99"
                  maskChar={null}
                  value={phoneNumber}
                  onChange={(e) => setPhoneNumber(e.target.value)}
                >
                  {(inputProps) => <input {...inputProps} className="form-input" required />}
                </InputMask>
              </div>

              <div className="address-group">
                <div className="form-group">
                  <label className="form-label">Город</label>
                  <input
                    type="text"
                    className="form-input"
                    value={city}
                    onChange={(e) => setCity(e.target.value)}
                    required
                  />
                </div>

                <div className="form-group">
                  <label className="form-label">Улица</label>
                  <input
                    type="text"
                    className="form-input"
                    value={street}
                    onChange={(e) => setStreet(e.target.value)}
                    required
                  />
                </div>

                <div className="form-group">
                  <label className="form-label">Дом</label>
                  <input
                    type="text"
                    className="form-input"
                    value={house}
                    onChange={(e) => setHouse(e.target.value)}
                    required
                  />
                </div>

                <div className="form-group">
                  <label className="form-label">Квартира</label>
                  <input
                    type="text"
                    className="form-input"
                    value={apartment}
                    onChange={(e) => setApartment(e.target.value)}
                  />
                </div>

                <div className="form-group">
                  <label className="form-label">Индекс</label>
                  <input
                    type="text"
                    className="form-input"
                    value={zipCode}
                    onChange={(e) => setZipCode(e.target.value)}
                    required
                  />
                </div>
              </div>

              <div className="form-group">
                <label className="form-label">Пароль</label>
                <input
                  type="password"
                  className="form-input"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                />
              </div>

              <div className="form-group">
                <label className="form-label">Подтвердите пароль</label>
                <input
                  type="password"
                  className="form-input"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                  required
                />
              </div>

              <button type="submit" className="auth-button">Зарегистрироваться</button>
            </form>

            <div className="auth-links">
              <Link to="/login" className="auth-link">Уже есть аккаунт? Войти</Link>
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default Register;