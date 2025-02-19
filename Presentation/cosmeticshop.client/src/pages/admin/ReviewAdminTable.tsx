import React, { useEffect, useState } from 'react';
import { AdminService } from '../../apiClient/http-services/admin.service';
import { ReviewResponseDto } from '../../apiClient/models/review-response-dto';
import { useNavigate } from 'react-router-dom';

const adminService = new AdminService('/api');

const ReviewAdminTable: React.FC = () => {
  const [reviews, setReviews] = useState<ReviewResponseDto[]>([]);
//   const [filterDto, setFilterDto] = useState({
//     filter: '',
//     sortField: '',
//     sortOrder: 'asc',
//     pageNumber: 1,
//     pageSize: 10
//   });
//   const [totalPages, setTotalPages] = useState<number>(1);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchReviews = async () => {
      try {
        const response = await adminService.getAllNotApprovedReviews(new AbortController().signal);
        setReviews(response);
        // setTotalPages(Math.ceil(response.totalItems / filterDto.pageSize));
      } catch (error) {
        console.error(error);
      }
    };

    fetchReviews();
  }, []);

//   const handleFilterChange = (e: React.ChangeEvent<HTMLInputElement>) => {
//     setFilterDto({
//       ...filterDto,
//       filter: e.target.value
//     });
//   };

//   const handleSortFieldChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
//     setFilterDto({
//       ...filterDto,
//       sortField: e.target.value
//     });
//   };

//   const handleSortOrderChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
//     setFilterDto({
//       ...filterDto,
//       sortOrder: e.target.value
//     });
//   };

//   const handlePageChange = (pageNumber: number) => {
//     setFilterDto({
//       ...filterDto,
//       pageNumber
//     });
//   };

//   const handlePageSizeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
//     setFilterDto({
//       ...filterDto,
//       pageSize: parseInt(e.target.value, 10)
//     });
//   };

  const handleApproveReview = async (reviewId: string) => {
    try {
      await adminService.approveReview(reviewId, new AbortController().signal);
      setReviews(reviews.map(review => (review.id === reviewId ? { ...review, isApproved: true } : review)));
    } catch (error) {
      console.error('Ошибка при одобрении отзыва:', error);
    }
  };

  const handleDeleteReview = async (reviewId: string) => {
    try {
      await adminService.deleteReview(reviewId, new AbortController().signal);
      setReviews(reviews.filter(review => review.id !== reviewId));
    } catch (error) {
      console.error('Ошибка при удалении отзыва:', error);
    }
  };

  return (
    <div className="admin-table-container">
      <h1 className="admin-title">Отзывы клиентов на рассмотрении</h1>
      {/* <div className="filters-container">
        <input
          type="text"
          placeholder="Поиск отзывов..."
          className="search-input"
          value={filterDto.filter}
          onChange={handleFilterChange}
        />
        <select
          className="filter-select"
          value={filterDto.sortField}
          onChange={handleSortFieldChange}
        >
          <option value="">Сортировать по</option>
          <option value="Rating">Рейтингу</option>
          <option value="DateCreated">Дате создания</option>
        </select>
        <select
          className="filter-select"
          value={filterDto.sortOrder}
          onChange={handleSortOrderChange}
        >
          <option value="asc">По возрастанию</option>
          <option value="desc">По убыванию</option>
        </select>
        <select
          className="filter-select"
          value={filterDto.pageSize}
          onChange={handlePageSizeChange}
        >
          <option value="10">10 на странице</option>
          <option value="25">25 на странице</option>
          <option value="50">50 на странице</option>
        </select>
      </div> */}
      <table className="admin-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Дата</th>
            <th>Товар</th>
            <th>Клиент</th>
            <th>Текст отзыва</th>
            <th>Рейтинг</th>
            <th>Статус</th>
            <th>Действия</th>
          </tr>
        </thead>
        <tbody>
          {reviews.length > 0 ? reviews.map(review => (
            <tr key={review.id}>
              <td>{review.id}</td>
              <td>{new Date(review.reviewDate).toISOString().split('T')[0]}</td>
              <td>{review.productName}</td>
              <td>{review.customerName}</td>
              <td>{review.reviewText}</td>
              <td>{review.rating}</td>
              <td>{review.isApproved ? 'Одобрен' : 'На модерации'}</td>
              <td>
                <button className="btn btn-outline" onClick={() => handleApproveReview(review.id)}>Одобрить</button>
                <button className="btn btn-primary" onClick={() => handleDeleteReview(review.id)}>Удалить</button>
              </td>
            </tr>
          )) : <p>Новых отзывов пока еще никто не написал! :))</p>}
        </tbody>
      </table>
      {/* <div className="pagination">
        <ul>
          {Array.from({ length: totalPages }, (_, index) => (
            <li key={index} onClick={() => handlePageChange(index + 1)}>
              {index + 1}
            </li>
          ))}
        </ul>
      </div> */}
    </div>
  );
};

export default ReviewAdminTable;