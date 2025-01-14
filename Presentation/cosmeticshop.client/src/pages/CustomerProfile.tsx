import React, { useEffect, useState } from 'react';
import { CustomerService } from '../apiClient/http-services/customer.service';
import { CustomerResponseDto } from '../apiClient/models';

const CustomerProfile: React.FC = () => {
  const [customer, setCustomer] = useState<CustomerResponseDto | null>(null);
  const customerService = new CustomerService('/api');

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await customerService.getCurrentCustomerProfile(new AbortController().signal);
        setCustomer(response);
      } catch (error) {
        console.error(error);
      }
    };

    fetchUser();
  }, []);

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
      {/* Возможность редактирования профиля */}
      {/* История заказов */}
    </div>
  );
};

export default CustomerProfile;