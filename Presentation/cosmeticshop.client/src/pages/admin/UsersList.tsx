import React, { useEffect, useState } from 'react';
import { AdminService } from '../../apiClient/http-services/admin.service';
import { RoleType, UserAddRequestDto, UserResponseDto } from '../../apiClient/models';
import { userRoleMap } from '../../apiClient/models/map/modelMaps';
import { useNavigate } from 'react-router-dom';
import "../../styles/admin/admin-global.css";
import "../../styles/admin/admin-user.css";


const adminService = new AdminService('/api');

const UsersList: React.FC = () => {
  const [users, setUsers] = useState<UserResponseDto[]>([]);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [filterDto, setFilterDto] = useState({
    filter: '',
    sortField: '',
    sortOrder: true,
    pageNumber: 1,
    pageSize: 10
  });
  const [totalPages, setTotalPages] = useState<number>(1);
  const [isAddUserFormVisible, setIsAddUserFormVisible] = useState(false);
  const [newUser, setNewUser] = useState<UserAddRequestDto>({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    role: RoleType.NUMBER_0
  });
  const navigate = useNavigate();

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const response = await adminService.getAllUsers(filterDto, new AbortController().signal);
        setUsers(response.items);
        setTotalPages(Math.ceil(response.totalItems / filterDto.pageSize));
      } catch (error) {
        console.error(error);
        setErrorMessage('Ошибка при загрузке пользователей');
        setTimeout(() => setErrorMessage(null), 4000);
      }
    };

    fetchUsers();
  }, [filterDto]);

  const handleFilterChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFilterDto({
      ...filterDto,
      filter: e.target.value
    });
  };

  const handleSortFieldChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setFilterDto({
      ...filterDto,
      sortField: e.target.value
    });
  };

  const handleSortOrderChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setFilterDto({
      ...filterDto,
      sortOrder: e.target.value === 'asc' ? true : false
    });
  };

  const handlePageChange = (pageNumber: number) => {
    setFilterDto({
      ...filterDto,
      pageNumber
    });
  };

  const handlePageSizeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setFilterDto({
      ...filterDto,
      pageSize: parseInt(e.target.value, 10)
    });
  };

  const handleUserDetails = (userId: string) => {
    navigate(`/admin/user/${userId}`);
  }

  const handleDeleteUser = async (userId: string) => {
    if (!confirm('Вы уверены, что хотите удалить этого пользователя?')) {
        return;
      }

    try {
      await adminService.deleteUser(userId, new AbortController().signal);
      setUsers(users.filter(user => user.id !== userId));
      setSuccessMessage('Пользователь успешно удален!');
      setTimeout(() => setSuccessMessage(null), 2000);  
    } catch (error) {
      console.error('Ошибка при удалении пользователя:', error);
      setErrorMessage('Ошибка при удалении пользователя! Попробуйте еще раз.');
      setTimeout(() => setErrorMessage(null), 2000);
    }
  };

  const handleAddUser = async () => {
    try {
      await adminService.addUser(newUser, new AbortController().signal);
      setUsers([...users, { ...newUser, id: Math.random().toString() } as UserResponseDto]);
      setSuccessMessage('Пользователь успешно добавлен!');
      setTimeout(() => setSuccessMessage(null), 2000);
      setIsAddUserFormVisible(false);
      setNewUser({ firstName: '', lastName: '', email: '', password: '', role: 0 });
    } catch (error) {
      console.error('Ошибка при добавлении пользователя:', error);
      setErrorMessage('Ошибка при добавлении пользователя! Попробуйте еще раз.');
      setTimeout(() => setErrorMessage(null), 2000);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setNewUser({
      ...newUser,
      [name]: name === 'role' ? Number(value) : value
    });
  };

   return (
    <div className="admin-table-container">
      <h1 className="admin-title">Пользователи</h1>
      {errorMessage && <div className="message error">{errorMessage}</div>}
      {successMessage && <div className="message success">{successMessage}</div>}
      <div className="add-button">
        <button className="btn btn-primary" onClick={() => setIsAddUserFormVisible(true)}>
          Добавить пользователя
        </button>
      </div>
      {isAddUserFormVisible && (
        <div className="user-add-form">
          <h2 className="admin-form-title">Добавить нового пользователя</h2>
          <div className="admin-form-group">
            <label>Имя:</label>
            <input
              type="text"
              name="firstName"
              value={newUser.firstName}
              onChange={handleInputChange}
              className="admin-form-input"
            />
          </div>
          <div className="admin-form-group">
            <label>Фамилия:</label>
            <input
              type="text"
              name="lastName"
              value={newUser.lastName}
              onChange={handleInputChange}
              className="admin-form-input"
            />
          </div>
          <div className="admin-form-group">
            <label>Email:</label>
            <input
              type="email"
              name="email"
              value={newUser.email}
              onChange={handleInputChange}
              className="admin-form-input"
            />
          </div>
          <div className="admin-form-group">
            <label>Password:</label>
            <input
              type="password"
              name="password"
              value={newUser.password}
              onChange={handleInputChange}
              className="admin-form-input"
            />
          </div>
          <div className="admin-form-group">
            <label>Роль:</label>
            <select
              name="role"
              value={newUser.role}
              onChange={handleInputChange}
              className="admin-role-select"
            >
              <option value={RoleType.NUMBER_0}>Администратор</option>
              <option value={RoleType.NUMBER_1}>Менеджер</option>
              <option value={RoleType.NUMBER_2}>Модератор</option>
            </select>
          </div>
          <div className='admin-form-button-secton'>
            <button className="btn btn-outline" onClick={handleAddUser}>
                Сохранить
            </button>
            <button className="btn btn-primary" onClick={() => setIsAddUserFormVisible(false)}>
                Отмена
            </button>            
          </div>

        </div>
      )}
      <div className="admin-filters-container">
        <input
          type="text"
          placeholder="Поиск пользователей..."
          className="admin-search-input"
          value={filterDto.filter}
          onChange={handleFilterChange}
        />
        <select
          className="admin-filter-select"
          value={filterDto.sortField}
          onChange={handleSortFieldChange}
        >
          <option value="">Сортировать по</option>
          <option value="FirstName">Имени</option>
          <option value="LastName">Фамилии</option>
          <option value="Email">Email</option>
        </select>
        <select
          className="admin-filter-select"
          value={filterDto.sortOrder ? 'asc' : 'desc'}
          onChange={handleSortOrderChange}
        >
          <option value="asc">По возрастанию</option>
          <option value="desc">По убыванию</option>
        </select>
        <select
          className="admin-filter-select"
          value={filterDto.pageSize}
          onChange={handlePageSizeChange}
        >
          <option value="10">10 на странице</option>
          <option value="25">25 на странице</option>
          <option value="50">50 на странице</option>
        </select>
      </div>
      <table className="admin-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Имя</th>
            <th>Фамилия</th>
            <th>Email</th>
            <th>Роль</th>
            <th>Действия</th>
          </tr>
        </thead>
        <tbody>
          {users.map(user => (
            <tr key={user.id}>
              <td>{user.id}</td>
              <td>{user.firstName}</td>
              <td>{user.lastName}</td>
              <td>{user.email}</td>
              <td>{userRoleMap[user.role]}
              </td>
              <td>
                <button className="btn btn-outline" onClick={() => handleUserDetails(user.id)}>
                    <i className="fas fa-search"></i>
                </button>
                <button className="btn btn-outline" onClick={() => handleDeleteUser(user.id)}>
                    <i className="fas fa-trash-alt"></i>
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      <div className="pagination">
        {Array.from({ length: totalPages }, (_, i) => (
          <button
            key={i + 1}
            className={`page-btn ${i + 1 === filterDto.pageNumber ? 'active' : ''}`}
            onClick={() => handlePageChange(i + 1)}
          >
            {i + 1}
          </button>
        ))}
      </div>
    </div>
  );
};


export default UsersList;