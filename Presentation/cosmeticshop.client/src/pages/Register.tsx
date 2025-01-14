import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../AuthContext';
import { CustomerRegisterRequestDto } from '../apiClient/models/customer-register-request-dto';
import InputMask from 'react-input-mask';

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
    <div>
      <h1>Регистрация</h1>
      {showSuccessMessage ? (
        <div>
          <p>Регистрация успешно завершена! <a href="/login">Войти?</a>.</p>
        </div>
      ) : (
        <form onSubmit={handleSubmit}>
          <div>
            <label>Имя:</label>
            <input
              type="text"
              value={firstName}
              onChange={(e) => setFirstName(e.target.value)}
              required
            />
          </div>
          <div>
            <label>Фамилия:</label>
            <input
              type="text"
              value={lastName}
              onChange={(e) => setLastName(e.target.value)}
              required
            />
          </div>
          <div>
            <label>Email:</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>
          <div>
            <label>Телефон:</label>
            <InputMask
              mask="+7(999)-999-99-99"
              maskChar={null}
              value={phoneNumber}
              onChange={(e) => setPhoneNumber(e.target.value)}
            >
              {(inputProps) => <input {...inputProps} type="tel" required />}
            </InputMask>
          </div>
          <div>
            <p>Адрес доставки:</p>
            <div>
              <label>Улица:</label>
              <input
                type="text"
                value={street}
                onChange={(e) => setStreet(e.target.value)}
                required
              />
            </div>
            <div>
              <label>Дом:</label>
              <input
                type="text"
                value={house}
                onChange={(e) => setHouse(e.target.value)}
                required
              />
            </div>
            <div>
              <label>Квартира:</label>
              <input
                type="text"
                value={apartment}
                onChange={(e) => setApartment(e.target.value)}
              />
            </div>
            <div>
              <label>Город:</label>
              <input
                type="text"
                value={city}
                onChange={(e) => setCity(e.target.value)}
                required
              />
            </div>
            <div>
              <label>Индекс:</label>
              <input
                type="text"
                value={zipCode}
                onChange={(e) => setZipCode(e.target.value)}
                required
              />
            </div>
          </div>
          <div>
            <label>Пароль:</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <div>
            <label>Подтвердите пароль:</label>
            <input
              type="password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
            />
          </div>
          <button type="submit">Зарегистрироваться</button>
        </form>
      )}
    </div>
  );
};

export default Register;