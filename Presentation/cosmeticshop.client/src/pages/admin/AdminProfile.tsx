import React, { useEffect, useState } from 'react';
import { UserService } from '../../apiClient/http-services/user.service';
import { AdminService } from '../../apiClient/http-services/admin.service';
import { UserResponseDto} from '../../apiClient/models';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import "../../styles/admin/AdminProfile.css";
import "../../styles/admin/admin-global.css";

const userService = new UserService('/api');
const adminService = new AdminService('/api');

const AdminProfile: React.FC = () => {
  const { isAuthenticated, role} = useAuth();
  const [admin, setAdmin] = useState<UserResponseDto | null>(null);
  const [orderCount, setOrderCount] = useState<number>(0);
  const [totalRevenue, setTotalRevenue] = useState<number>(0);
  const [newCustomerCount, setNewCustomerCount] = useState<number>(0);
  const [approvedReviewCount, setApprovedReviewCount] = useState<number>(0);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const navigate = useNavigate();

  const roleMap: { [key: number]: string } = {
    0: 'Администратор',
    1: 'Менеджер',
    2: 'Модератор',
  };

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

    const fetchStatistic = async () => {
      try {
        const response = await adminService.getStatistic(new AbortController().signal);
        setOrderCount(response.orderCount);
        setTotalRevenue(response.totalRevenue);
        setNewCustomerCount(response.newCustomerCount);
        setApprovedReviewCount(response.approvedReviewCount);
      } catch (error) {
        console.error(error);
        setErrorMessage('Ошибка при загрузке количества заказов');
      }
    };

     fetchAdminProfile();
     fetchStatistic();
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
            <span>{roleMap[admin.role]}</span>
          </div>
        </div>
      )}
      <div className="admin-links">
        <Link to="/admin/products" className="btn btn-outline">Управление товарами</Link>
        <Link to="/admin/orders" className="btn btn-outline">Управление заказами</Link>
        <Link to="/admin/reviews" className="btn btn-outline">Управление отзывами</Link>
        <Link to="/admin/users" className="btn btn-outline">Управление пользователями</Link>
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
              <td>{orderCount}</td>
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