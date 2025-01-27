import React, { useEffect, useState } from 'react';
import { CustomerService } from '../apiClient/http-services/customer.service';
import { OrderService } from '../apiClient/http-services/order.service'
import { CustomerResponseDto, OrderResponseDto } from '../apiClient/models';
import '../styles/Profile.css'

const CustomerProfile: React.FC = () => {
  const [customer, setCustomer] = useState<CustomerResponseDto | null>(null);
  const [orders, setOrders] = useState<OrderResponseDto[]>([]);
  const [isOrdersExpanded, setIsOrdersExpanded] = useState<boolean>(false);
  const [isEditProfile, setIsEditProfile] = useState<boolean>(false);
  const [isResetPassword, setIsResetPassword] = useState<boolean>(false);
  const customerService = new CustomerService('/api');
  const orderService = new OrderService('/api');

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await customerService.getCurrentCustomerProfile(new AbortController().signal);
        setCustomer(response);
      } catch (error) {
        console.error(error);
      }
    };

    const fetchOrders = async () => {
      try {
        const response = await orderService.getCustomerOrdersAsync(new AbortController().signal);
        setOrders(response);
      } catch (error) {
        console.error(error);
      }
    };

    fetchUser();
    fetchOrders();
  }, []);

  const handleEditProfile = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!customer) return;

    const formData = new FormData(e.currentTarget as HTMLFormElement);
    const firstName = formData.get('firstName') as string;
    const lastName = formData.get('lastName') as string;
    const email = formData.get('email') as string;
    const phoneNumber = formData.get('phoneNumber') as string;
    const shippingAddress = formData.get('shippingAddress') as string;

    try {
      const updatedCustomer = await customerService.updateCustomerProfile(
        email,
        firstName,
        lastName,
        phoneNumber,
        shippingAddress,
        new AbortController().signal
      );
      setCustomer(updatedCustomer);
      setIsEditProfile(false);
    } catch (error) {
      console.error(error);
    }
  };

  
  const handleResetPassword = async (e: React.FormEvent) => {
    e.preventDefault();
    const formData = new FormData(e.currentTarget as HTMLFormElement);
    const newPassword = formData.get('newPassword') as string;
    const confirmPassword = formData.get('confirmPassword') as string;

    if (newPassword !== confirmPassword) {
      alert('Пароли не совпадают');
      return;
    }

    try {
      await customerService.resetPassword(newPassword, new AbortController().signal);
      alert('Пароль успешно обновлен');
      setIsResetPassword(false);
    } catch (error) {
      console.error(error);
      alert('Ошибка при обновлении пароля');
    }
  };

  if (!customer) {
    return <div>Загрузка...</div>;
  }

  return (
    <div className="profile-container">
      <div className="profile-card">
        <h1 className="profile-title">Мой профиль</h1>
        
        <div className="profile-info">
          <div className="profile-picture">
            <img src={'../../images/foto.jpg'} alt="Фото профиля" />
          </div>
          <div className="info-group">
            <span className="info-label">Имя:</span>
            <span className="info-value">{customer.firstName} {customer.lastName}</span>
          </div>
          <div className="info-group">
            <span className="info-label">Email:</span>
            <span className="info-value">{customer.email}</span>
          </div>
          <div className="info-group">
            <span className="info-label">Телефон:</span>
            <span className="info-value">{customer.phoneNumber || 'Не указан'}</span>
          </div>
          <div className="info-group">
            <span className="info-label">Адрес доставки:</span>
            <span className="info-value">{customer.shippingAddress || 'Не указан'}</span>
          </div>
        </div>

        <div className="profile-actions">
          <button 
            className="action-btn edit-btn"
            onClick={() => setIsEditProfile(true)}
          >
            Редактировать профиль
          </button>
          <button 
            className="action-btn reset-btn"
            onClick={() => setIsResetPassword(true)}
          >
            Сменить пароль
          </button>
        </div>

        {isEditProfile && (
          <form className="edit-form" onSubmit={handleEditProfile}>
            <h3 className="form-title">Редактирование профиля</h3>
            <div className="form-grid">
              <div className="form-group">
                <label>Имя</label>
                <input type="text" name="firstName" defaultValue={customer.firstName} required />
              </div>
              <div className="form-group">
                <label>Фамилия</label>
                <input type="text" name="lastName" defaultValue={customer.lastName} required />
              </div>
              <div className="form-group">
                <label>Email</label>
                <input type="email" name="email" defaultValue={customer.email} required />
              </div>
              <div className="form-group">
                <label>Телефон</label>
                <input type="text" name="phoneNumber" defaultValue={customer.phoneNumber || ''} />
              </div>
              <div className="form-group full-width">
                <label>Адрес доставки</label>
                <input type="text" name="shippingAddress" defaultValue={customer.shippingAddress || ''} />
              </div>
            </div>
            <div className="form-buttons">
              <button type="submit" className="save-btn">Сохранить</button>
              <button type="button" className="cancel-btn" onClick={() => setIsEditProfile(false)}>
                Отмена
              </button>
            </div>
          </form>
        )}

        {isResetPassword && (
          <form className="password-form" onSubmit={handleResetPassword}>
            <h3 className="form-title">Смена пароля</h3>
            <div className="form-group">
              <label>Новый пароль</label>
              <input type="password" name="newPassword" required />
            </div>
            <div className="form-group">
              <label>Подтвердите пароль</label>
              <input type="password" name="confirmPassword" required />
            </div>
            <div className="form-buttons">
              <button type="submit" className="save-btn">Сохранить</button>
              <button type="button" className="cancel-btn" onClick={() => setIsResetPassword(false)}>
                Отмена
              </button>
            </div>
          </form>
        )}

        <div className="orders-section">
          <div className="orders-header" onClick={() => setIsOrdersExpanded(!isOrdersExpanded)}>
            <h2>История заказов</h2>
            <span className={`arrow ${isOrdersExpanded ? 'up' : 'down'}`}></span>
          </div>
          
          {isOrdersExpanded && (
            <div className="orders-list">
              {orders.length === 0 ? (
                <p className="no-orders">У вас пока нет заказов</p>
              ) : (
                orders.map(order => (
                  <div key={order.id} className="order-card">
                    <div className="order-header">
                      <span className="order-number">Заказ №{order.id}</span>
                      <span className={`order-status ${order.status}`}>
                        {order.status}
                      </span>
                    </div>
                    <div className="order-details">
                      <div className="order-date">
                        Дата: {new Date(order.orderDate).toLocaleDateString('ru-RU')}
                      </div>
                      <div className="order-amount">
                        Сумма: {order.totalAmount} ₽
                      </div>
                    </div>
                  </div>
                ))
              )}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default CustomerProfile;