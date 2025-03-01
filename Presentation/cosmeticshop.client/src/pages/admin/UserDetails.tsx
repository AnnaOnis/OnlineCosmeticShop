import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { AdminService } from '../../apiClient/http-services/admin.service';
import { RoleType, UserResponseDto, } from '../../apiClient/models';
import "../../styles/admin/admin-global.css";
import "../../styles/admin/admin-user.css";
import { AdminUserUpdateRequestDto } from '../../apiClient/models/admin-user-update-request-dto';
import { userRoleMap } from '../../apiClient/models/map/modelMaps';

const adminService = new AdminService('/api');

const UserDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [user, setUser] = useState<UserResponseDto | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [isEditMode, setIsEditMode] = useState<boolean>(false); 
  const [updatedUser, setUpdatedUser] = useState<AdminUserUpdateRequestDto | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await adminService.getUserById(id!, new AbortController().signal);
        setUser(response);
        const newUpdatedUser: AdminUserUpdateRequestDto = {
          firstName: response.firstName,
          lastName: response.lastName,
          email: response.email,
          userRole: response.role
        };
        setUpdatedUser(newUpdatedUser); 
      } catch (error) {
        console.error(error);
        setErrorMessage('Ошибка при загрузке информации о пользователе');
      }
    };

    fetchUser();
  }, [id]);

  if (!user || !updatedUser) return <div>Загрузка...</div>;

  const handleDeleteUser = async (userId: string) => {
    if (!confirm('Вы уверены, что хотите удалить этого пользователя?')) {
        return;
      }

    try {
      await adminService.deleteUser(userId, new AbortController().signal);
      setSuccessMessage('Пользователь успешно удален!');
      setTimeout(() => setSuccessMessage(null), 2000);
      setTimeout(() => navigate("/admin/users"), 2500);
      ;      
    } catch (error) {
      console.error('Ошибка при удалении пользователя:', error);
      setErrorMessage('Ошибка при удалении пользователя! Попробуйте еще раз.');
      setTimeout(() => setErrorMessage(null), 2000);
    }
  };

  const toggleEditMode = () => {
    setIsEditMode(!isEditMode); // Переключаем режим редактирования
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    if (updatedUser) {
      setUpdatedUser({
        ...updatedUser,
        [name]: name === 'userRole' ? Number(value) : value
      });
    }
  };

  const handleUpdateUser = async (userId: string) => {
    if (!updatedUser) return;

    try {
      console.log(updatedUser);
      const response = await adminService.updateUser(userId, updatedUser, new AbortController().signal);
      setUser(response); // Обновляем пользователя после успешного обновления
      setSuccessMessage('Данные пользователя успешно обновлены!');
      setTimeout(() => setSuccessMessage(null), 2000);
      setIsEditMode(false); // Возвращаемся к режиму просмотра
    } catch (error) {
      console.error('Ошибка при обновлении пользователя:', error);
      setErrorMessage('Ошибка при обновлении пользователя! Попробуйте еще раз.');
      setTimeout(() => setErrorMessage(null), 2000);
    }
  };

  return (
    <div className="user-details-container">
      <h1 className="admin-title">Информация о пользователе</h1>
      {errorMessage && <div className="message error">{errorMessage}</div>}
      {successMessage && <div className="message success">{successMessage}</div>}
      {isEditMode ? (
        // Форма для редактирования пользователя
        <form className='update-user-form'>
          <div className="admin-form-group">
            <label>ID:</label>
            <input name="id" 
                   type="text" 
                   value={user.id || ''} 
                   onChange={handleChange} readOnly 
                   className="admin-form-input"/>
          </div>
          <div className="admin-form-group">
            <label>Имя:</label>
            <input name="firstName" 
                   type="text" 
                   value={updatedUser.firstName || ''} 
                   onChange={handleChange} 
                   className="admin-form-input"/>
          </div>
          <div className="admin-form-group">
            <label>Фамилия:</label>
            <input name="lastName" 
                   type="text" 
                   value={updatedUser.lastName || ''} 
                   onChange={handleChange} 
                   className="admin-form-input"/>
          </div>
          <div className="admin-form-group">
            <label>Email:</label>
            <input name="email" 
                   type="text" 
                   value={updatedUser.email || ''} 
                   onChange={handleChange} 
                   className="admin-form-input"/>
          </div>
          <div className="admin-form-group">
            <label>Роль:</label>
            <select
              name="userRole"
              value={updatedUser.userRole}
              onChange={handleChange}
              className="admin-role-select"
            >
              <option value={RoleType.NUMBER_0}>Администратор</option>
              <option value={RoleType.NUMBER_1}>Менеджер</option>
              <option value={RoleType.NUMBER_2}>Модератор</option>
            </select>
          </div>
          <div className='admin-form-button-secton'>
            <button className="btn btn-outline" type="button" onClick={() => handleUpdateUser(user.id)}>
              Сохранить
            </button>
            <button className="btn btn-primary" type="button" onClick={toggleEditMode}>
              Отмена
            </button>            
          </div>
        </form>
      ) : (
      <div className="user-info">
        <div className="user-field">
          <label>ID:</label>
          <span>{user.id}</span>
        </div>
        <div className="user-field">
          <label>Имя:</label>
          <span>{user.firstName}</span>
        </div>
        <div className="user-field">
          <label>Фамилия:</label>
          <span>{user.lastName}</span>
        </div>
        <div className="user-field">
          <label>Email:</label>
          <span>{user.email}</span>
        </div>
        <div className="user-field">
          <label>Роль:</label>
          <span>{userRoleMap[user.role]}</span>
        </div>
        <div className='admin-form-button-secton'>
          <button className="btn btn-outline" onClick={toggleEditMode}>Изменить</button>
          <button className="btn btn-outline" onClick={() => handleDeleteUser(id!)}>Удалить</button>           
        </div>       
      </div>)}

    </div>
  );
};

export default UserDetails;