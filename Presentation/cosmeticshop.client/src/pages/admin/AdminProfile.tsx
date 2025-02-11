import React, { useEffect, useState } from 'react';
import { UserService } from '../../apiClient/http-services/user.service';
import { OrderService } from '../../apiClient/http-services/order.service';
import { ReviewsService } from '../../apiClient/http-services/reviews.service';
import { UserResponseDto } from '../../apiClient/models';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import "../../styles/admin/AdminProfile.css";

const userService = new UserService('/api');
const orderService = new OrderService('/api');
const reviewsService = new ReviewsService('/api');

const AdminProfile: React.FC = () => {
    const { isAuthenticated, role} = useAuth();
  const [admin, setAdmin] = useState<UserResponseDto | null>(null);
  const [orderIdCount, setOrderIdCount] = useState<number>(0);
  const [totalRevenue, setTotalRevenue] = useState<number>(0);
  const [newCustomerCount, setNewCustomerCount] = useState<number>(0);
  const [approvedReviewCount, setApprovedReviewCount] = useState<number>(0);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchAdminProfile = async () => {
        if (!isAuthenticated || role == null) {
            navigate('/login');
            return;
          }
      try {
        const adminId = localStorage.getItem('id');
        if (!adminId) {
          throw new Error('Администратор не авторизован');
        }
        const response = await userService.getCurrentUserProfile(new AbortController().signal);
        setAdmin(response);
      } catch (error) {
        console.error(error);
        setErrorMessage('Ошибка при загрузке профиля администратора');
        navigate('/login');
      }
    };

    // const fetchOrderIdCount = async () => {
    //   try {
    //     const response = await orderService.getOrdersCount(new AbortController().signal);
    //     setOrderIdCount(response);
    //   } catch (error) {
    //     console.error(error);
    //     setErrorMessage('Ошибка при загрузке количества заказов');
    //   }
    // };

    // const fetchTotalRevenue = async () => {
    //   try {
    //     const response = await orderService.getTotalRevenue(new AbortController().signal);
    //     setTotalRevenue(response);
    //   } catch (error) {
    //     console.error(error);
    //     setErrorMessage('Ошибка при загрузке общей выручки');
    //   }
    // };

    // const fetchNewCustomerCount = async () => {
    //   try {
    //     const response = await userService.getNewCustomersCount(new AbortController().signal);
    //     setNewCustomerCount(response);
    //   } catch (error) {
    //     console.error(error);
    //     setErrorMessage('Ошибка при загрузке количества новых клиентов');
    //   }
    // };

    // const fetchApprovedReviewCount = async () => {
    //   try {
    //     const response = await reviewsService.getApprovedReviewsCount(new AbortController().signal);
    //     setApprovedReviewCount(response);
    //   } catch (error) {
    //     console.error(error);
    //     setErrorMessage('Ошибка при загрузке количества одобренных отзывов');
    //   }
    // };

     fetchAdminProfile();
    // fetchOrderIdCount();
    // fetchTotalRevenue();
    // fetchNewCustomerCount();
    // fetchApprovedReviewCount();
  }, [navigate]);

  return (
    <div className="admin-profile-container">
      <h1 className="admin-title">Профиль администратора</h1>
      {errorMessage && <div className="message error">{errorMessage}</div>}
      {admin && (
        <div className="admin-profile-info">
          <div className="admin-field">
            <label>Имя:</label>
            <span>{admin.firstName}</span>
          </div>
          <div className="admin-field">
            <label>Фамилия:</label>
            <span>{admin.lastName}</span>
          </div>
          <div className="admin-field">
            <label>Email:</label>
            <span>{admin.email}</span>
          </div>
          <div className="admin-field">
            <label>Роль:</label>
            <span>{admin.role}</span>
          </div>
        </div>
      )}
      <h2 className="admin-subtitle">Функции для работы</h2>
      <div className="admin-links">
        <Link to="/product-admin-table" className="btn btn-outline">Управление товарами</Link>
        <Link to="/order-admin-table" className="btn btn-outline">Управление заказами</Link>
        <Link to="/review-admin-table" className="btn btn-outline">Управление отзывами</Link>
        <Link to="/user-admin-table" className="btn btn-outline">Управление пользователями</Link>
      </div>
      <h2 className="admin-subtitle">Статистика и аналитика</h2>
      <div className="admin-statistics">
        <table className="admin-table">
          <thead>
            <tr>
              <th>Показатель</th>
              <th>Значение</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>Общее количество заказов</td>
              <td>{orderIdCount}</td>
            </tr>
            <tr>
              <td>Общая выручка</td>
              <td>{totalRevenue.toLocaleString()} ₽</td>
            </tr>
            <tr>
              <td>Количество новых клиентов</td>
              <td>{newCustomerCount}</td>
            </tr>
            <tr>
              <td>Количество одобренных отзывов</td>
              <td>{approvedReviewCount}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default AdminProfile;