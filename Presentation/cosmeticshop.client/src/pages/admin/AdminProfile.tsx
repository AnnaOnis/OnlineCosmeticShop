import React, { useEffect, useState } from 'react';
import { UserService } from '../../apiClient/http-services/user.service';
import { AdminService } from '../../apiClient/http-services/admin.service';
import { PasswordResetRequestDto, RoleType, UserResponseDto, UserUpdateRequestDto} from '../../apiClient/models';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import "../../styles/admin/AdminProfile.css";
import "../../styles/admin/admin-global.css";
import { userRoleMap } from '../../apiClient/models/map/modelMaps';

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
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [isEditProfileOpen, setIsEditProfileOpen] = useState<boolean>(false);
  const [isChangePasswordOpen, setIsChangePasswordOpen] = useState<boolean>(false);
  const [newFirstName, setNewFirstName] = useState<string>('');
  const [newLastName, setNewLastName] = useState<string>('');
  const [newEmail, setNewEmail] = useState<string>('');
  const [confirmPassword, setConfirmPassword] = useState<string>('');
  const [newPassword, setNewPassword] = useState<string>('');
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
        setTimeout(() => setErrorMessage(null), 2000);
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
        setTimeout(() => setErrorMessage(null), 2000);
      }
    };

     fetchAdminProfile();
     fetchStatistic();
  }, [navigate]);

  useEffect(() => {
    if (isEditProfileOpen && admin) {
      setNewFirstName(admin.firstName);
      setNewLastName(admin.lastName);
      setNewEmail(admin.email);
    }
  }, [isEditProfileOpen, admin]);

  const isAdministrator = role === RoleType.NUMBER_0;
  const isManager = role === RoleType.NUMBER_1;
  const isModerator = role === RoleType.NUMBER_2;

  const handleEditProfile = async () => {
    if (!admin) return;
    try {
      const updatedUser: UserUpdateRequestDto = {
        firstName: newFirstName,
        lastName: newLastName,
        email: newEmail,
      };
      const response = await userService.updateUserProfile(updatedUser, new AbortController().signal);
      setAdmin(response);
      setIsEditProfileOpen(false);
      setSuccessMessage('Данные профиля успешно обновлены!');
      setTimeout(() => setSuccessMessage(null), 2000);
    } catch (error) {
      console.error(error);
      setErrorMessage('Ошибка при обновлении профиля');
      setTimeout(() => setErrorMessage(null), 2000);
    }
  };

  const handleChangePassword = async () => {
    if(newPassword !== confirmPassword){
      alert('Пароли не совпадают! Проверьте правильность введенных данных и попробуйте еще раз!');
      return;
    }
    try {
      const passwordResetRequest: PasswordResetRequestDto = {
        newPassword,
      };
      await userService.resetPassword(passwordResetRequest, new AbortController().signal);
      setIsChangePasswordOpen(false);
      setConfirmPassword('');
      setNewPassword('');
      setSuccessMessage('Пароль успешно изменен!');
      setTimeout(() => setSuccessMessage(null), 2000);
    } catch (error) {
      console.error(error);
      setErrorMessage('Ошибка при смене пароля');
      setTimeout(() => setErrorMessage(null), 2000);
    }
  };

  return (
    <div className="admin-profile-container">
      <h1 className="admin-title">Профиль администратора</h1>
      {errorMessage && <div className="message error">{errorMessage}</div>}
      {successMessage && <div className="message success">{successMessage}</div>}
      {admin && (
        <>
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
              <span>{userRoleMap[admin.role]}</span>
            </div><br/>
            <div className='admin-form-button-secton'>
              <button className="btn btn-primary" onClick={() => setIsEditProfileOpen(true)}>Редактировать профиль</button>
              <button className="btn btn-primary" onClick={() => setIsChangePasswordOpen(true)}>Сменить пароль</button>
            </div><br/>          
           
            {isEditProfileOpen && (
              <div className="update-user-form">
                <h2 className="admin-subtitle">Редактирование профиля</h2>
                <form className="admin-form">
                  <div className="admin-form-group">
                    <label>Имя:</label>
                    <input type="text" value={newFirstName} onChange={(e) => setNewFirstName(e.target.value)} className="admin-form-input" />
                  </div>
                  <div className="admin-form-group">
                    <label>Фамилия:</label>
                    <input type="text" value={newLastName} onChange={(e) => setNewLastName(e.target.value)} className="admin-form-input" />
                  </div>
                  <div className="admin-form-group">
                    <label>Email:</label>
                    <input type="email" value={newEmail} onChange={(e) => setNewEmail(e.target.value)} className="admin-form-input" />
                  </div>
                  <div className='admin-form-button-secton'>
                    <button type="button" onClick={handleEditProfile} className="btn btn-outline">Сохранить изменения</button>
                    <button type="button" onClick={() => setIsEditProfileOpen(false)} className="btn btn-primary">Отмена</button>
                  </div>                  
                </form>
              </div>
            )}
            {isChangePasswordOpen && (
              <div className="update-user-form">
                <h2 className="admin-subtitle">Смена пароля</h2>
                <form className="admin-form">
                  <div className="admin-form-group">
                    <label>Новый пароль:</label>
                    <input type="password" value={newPassword} onChange={(e) => setNewPassword(e.target.value)} className="admin-form-input" />
                  </div>
                  <div className="admin-form-group">
                    <label>Подтвердить пароль:</label>
                    <input type="password"  value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)} className="admin-form-input" />
                  </div>
                  <div className='admin-form-button-secton'>
                    <button type="button" onClick={handleChangePassword} className="btn btn-outline">Сохранить изменения</button>
                    <button type="button" onClick={() => setIsChangePasswordOpen(false)} className="btn btn-primary">Отмена</button>
                  </div>                 
                </form>
              </div>
            )}
          </div> 
        </>
      )}
      <div className="admin-links">
        {isAdministrator && (
          <Link to="/admin/users" className="btn btn-outline">Управление пользователями</Link>
        )}
        {(isAdministrator || isManager) && (
          <>
            <Link to="/admin/products" className="btn btn-outline">Управление товарами</Link>
            <Link to="/admin/orders" className="btn btn-outline">Управление заказами</Link>               
          </>   
        )}
        {(isAdministrator || isModerator) && ( 
          <Link to="/admin/reviews" className="btn btn-outline">Управление отзывами</Link>
        )}       
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