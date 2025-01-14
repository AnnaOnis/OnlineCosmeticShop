import React, { useEffect, useState } from 'react';
import axios from 'axios';

const CustomerProfile: React.FC = () => {
  const [customer, setUser] = useState<{ name: string; email: string } | null>(null);

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await axios.get('/api/user');
        setUser(response.data);
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
      <p>Имя: {customer.name}</p>
      <p>Email: {customer.email}</p>
      {/* Возможность редактирования профиля */}
      {/* История заказов */}
    </div>
  );
};

export default CustomerProfile;