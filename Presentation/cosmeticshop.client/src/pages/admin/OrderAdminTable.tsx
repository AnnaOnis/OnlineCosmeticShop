import React, { useEffect, useState } from 'react';
import { AdminService } from '../../apiClient/http-services/admin.service';
import { OrderResponseDto, OrderUpdateRequestDto } from '../../apiClient/models';
import { useNavigate } from 'react-router-dom';
import "../../styles/admin/admin-global.css"
import { orderPaymentStatusMap, orderStatusMap } from '../../apiClient/models/map/modelMaps';
import { PaymentUpdateStatusRequestDto } from '../../apiClient/models/payment-update-status-request-dto';

const adminService = new AdminService('/api');

const OrderAdminTable: React.FC = () => {
  const [orders, setOrders] = useState<OrderResponseDto[]>([]);
  const [filterDto, setFilterDto] = useState({
    filter: '',
    sortField: '',
    sortOrder: true,
    pageNumber: 1,
    pageSize: 10
  });
  const [totalPages, setTotalPages] = useState<number>(1);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const response = await adminService.getAllOrdersAsync(filterDto, new AbortController().signal);
        setOrders(response.items);
        setTotalPages(Math.ceil(response.totalItems / filterDto.pageSize));
      } catch (error) {
        console.error(error);
      }
    };

    fetchOrders();
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

  const handleUpdateOrderStatus = async (orderId: string | undefined, status: number) => {
    if(orderId === undefined)return;
    try {
      const updateData: OrderUpdateRequestDto = {
        id: orderId,
        newStatus: status
      };
      await adminService.updateOrderStatusAsync(updateData, new AbortController().signal);
      setOrders(orders.map(order => (order.id === orderId ? { ...order, status } : order)));
    } catch (error) {
      console.error('Ошибка при обновлении статуса заказа:', error);
    }
  };

  const handleUpdatePaymentStatus = async (orderId: string | undefined, status: number) => {
    if(orderId === undefined)return;
    try{
        const updateData: PaymentUpdateStatusRequestDto = {
            orderId: orderId,
            newPaymentStatus: status
        };
        await adminService.updatePaymentStatusByOrder(updateData, new AbortController().signal);
        setOrders(orders.map(order => (order.id === orderId ? { ...order, orderPaymentStatus: status } : order)));
    }catch (error) {
      console.error('Ошибка при обновлении статуса платежа по заказу:' + orderId, error);
    }}

  return (
    <div className="admin-table-container">
      <h1 className="admin-title">Заказы клиентов</h1>
      <div className="admin-filters-container">
        <input
          type="text"
          placeholder="Поиск заказов..."
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
          <option value="TotalAmount">Сумме заказа</option>
          <option value="TotalQuantity">Количеству товаров</option>
          <option value="DateCreated">Дате создания</option>
          <option value="Status">Статусу</option>
        </select>
        <select
          className="admin-filter-select"
          value={filterDto.sortOrder ? "asc" : "desc"}
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
            <th>Дата</th>
            <th>Сумма заказа</th>
            <th>Количество товаров</th>
            <th>Статус</th>
            <th>Изменить статус заказа</th>
            <th>Статус платежа</th>   
            <th>Изменить статус платежа</th>         
          </tr>
        </thead>
        <tbody>
          {orders.length > 0 ? orders.map(order => (
            <tr key={order.id}>
              <td>{order.id}</td>
              <td>{new Date(order.orderDate).toLocaleDateString("ru-Ru")}</td>
              <td>{order.totalAmount !== undefined ? order.totalAmount.toLocaleString() : 'Нет данных'} ₽</td>
              <td>{order.totalQuantity !== undefined ? order.totalQuantity : 'Нет данных'}</td>
              <td className={`admin-order-status ${order.status !== undefined ? order.status : ''}`}>
                {order.status !== undefined ? orderStatusMap[order.status] : 'Нет данных'}
              </td>
              <td>
                <select
                  value={order.status}
                  onChange={e => handleUpdateOrderStatus(order.id, parseInt(e.target.value, 10))}
                  className="admin-filter-select"
                >
                  {Object.keys(orderStatusMap).map(key => (
                    <option key={key} value={key}>
                      {orderStatusMap[parseInt(key, 10)]}
                    </option>
                  ))}
                </select>
              </td>
              <td className={`admin-payment-status ${order.orderPaymentStatus !== undefined ? order.orderPaymentStatus : ''}`}>
                {order.orderPaymentStatus !== undefined ? orderPaymentStatusMap[order.orderPaymentStatus] : 'Нет данных'}
              </td>
              <td>
                <select
                  value={order.orderPaymentStatus}
                  onChange={e => handleUpdatePaymentStatus(order.id, parseInt(e.target.value, 10))}
                  className="admin-filter-select"
                >
                  {Object.keys(orderPaymentStatusMap).map(key => (
                    <option key={key} value={key}>
                      {orderPaymentStatusMap[parseInt(key, 10)]}
                    </option>
                  ))}
                </select>
              </td>
            </tr>
          )) : <p>Список заказов пуст.</p>}
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

export default OrderAdminTable;