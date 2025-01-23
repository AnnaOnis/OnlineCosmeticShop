import React, { useEffect, useState } from 'react';
import { CustomerService } from '../apiClient/http-services/customer.service';
import { OrderService } from '../apiClient/http-services/order.service'
import { CustomerResponseDto, OrderResponseDto } from '../apiClient/models';

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
    <div>
      <h1>Профиль пользователя</h1>
      <p>Имя: {customer.firstName} {customer.lastName}</p>
      <p>Email: {customer.email}</p>
      <p>Номер телефона: {customer.phoneNumber}</p>
      <p>Адрес доставки: {customer.shippingAddress}</p>
      <button onClick={() => setIsEditProfile(true)}>Редактировать профиль</button>
      <button onClick={() => setIsResetPassword(true)}>Сбросить пароль</button>

      {/* Возможность редактирования профиля */}
      {isEditProfile && (
        <form onSubmit={handleEditProfile}>
          <div>
            <label>Имя: </label>
            <input type="text" name="firstName" defaultValue={customer.firstName} required />
          </div>
          <div>
            <label>Фамилия: </label>
            <input type="text" name="lastName" defaultValue={customer.lastName} required />
          </div>
          <div>
            <label>Email: </label>
            <input type="email" name="email" defaultValue={customer.email} required />
          </div>
          <div>
            <label>Номер телефона: </label>
            <input type="text" name="phoneNumber" defaultValue={customer.phoneNumber || ''} />
          </div>
          <div>
            <label>Адрес доставки: </label>
            <input type="text" name="shippingAddress" defaultValue={customer.shippingAddress || ''} />
          </div>
          <button type="submit">Обновить профиль</button>
          <button type="button" onClick={() => setIsEditProfile(false)}>Отмена</button>
        </form>
      )}

      {isResetPassword && (
        <form onSubmit={handleResetPassword}>
          <div>
            <label>Новый пароль: </label>
            <input type="password" name="newPassword" required />
          </div>
          <div>
            <label>Подтвердите новый пароль: </label>
            <input type="password" name="confirmPassword" required />
          </div>
          <button type="submit">Сбросить пароль</button>
          <button type="button" onClick={() => setIsResetPassword(false)}>Отмена</button>
        </form>
      )}

      {/* История заказов */}
      <div style={{ display: 'flex', alignItems: 'center' }}>
        <h2>История заказов</h2>
        <button onClick={() => setIsOrdersExpanded(!isOrdersExpanded)} style={{ marginLeft: '10px' }}>
          {isOrdersExpanded ? '\u25B2' : '\u25BC'}
        </button>
      </div>
      {isOrdersExpanded && (
        <>
          {orders.length === 0 ? (
            <p>У вас пока нет заказов.</p>
          ) : (
            <ul>
              {orders.map(order => (
                <li key={order.id}>
                  <strong>Заказ №{order.id}</strong>
                  <p>Дата: {new Date(order.orderDate).toLocaleDateString()}</p>
                  <p>Статус: {order.status}</p>
                  <p>Сумма: {order.totalAmount} руб.</p>
                </li>
              ))}
            </ul>
          )}
        </>
      )}
    </div>
  );
};

export default CustomerProfile;