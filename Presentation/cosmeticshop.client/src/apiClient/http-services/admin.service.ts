import HttpClient from '../httpClient';
import { CustomerResponseDto, FilterDto, 
         OrderResponseDto, 
         OrderUpdateRequestDto,
         PagedResponse, 
         ProductRequestDto, 
         ProductResponseDto, 
         ReviewResponseDto,
         UserAddRequestDto,
         UserResponseDto} from '../models';
import { AdminUserUpdateRequestDto } from '../models/admin-user-update-request-dto';
import { PaymentUpdateStatusRequestDto } from '../models/payment-update-status-request-dto';
import { StatisticResponse } from '../models/statistic-response';

export class AdminService{
    private httpClient: HttpClient;

    constructor(basePath: string) {
      this.httpClient = new HttpClient(basePath);
    }


    //  --  Работа с товарами  --
      // Создание нового продукта
    public async createProduct(productRequestDto: ProductRequestDto, cancellationToken: AbortSignal): Promise<ProductResponseDto> {
      return await this.httpClient.post<ProductResponseDto>('/admin/product/create', productRequestDto, { signal: cancellationToken });
    }
    // Обновление продукта по ID
    public async updateProduct(id: string, productRequestDto: ProductRequestDto, cancellationToken: AbortSignal): Promise<ProductResponseDto> {
      return await this.httpClient.put<ProductResponseDto>(`/admin/product/${id}`, productRequestDto, { signal: cancellationToken });
    }
    
    // Удаление продукта по ID
    public async deleteProduct(id: string, cancellationToken: AbortSignal): Promise<void> {
      await this.httpClient.delete(`/admin/product/${id}`, { signal: cancellationToken });
    }



    //  --  Работа с заказами и платежами  -- 
     // Получение всех заказов с фильтрацией, сортировкой и пагинацией
    public async getAllOrdersAsync(filterDto: FilterDto, cancellationToken: AbortSignal): Promise<PagedResponse<OrderResponseDto>> {
      const params = new URLSearchParams();
      for (const [key, value] of Object.entries(filterDto)) {
        if (value !== undefined && value !== null) {
          params.append(key, String(value));
        }
      }
      const response = await this.httpClient.get<PagedResponse<OrderResponseDto>>('/admin/orders', { params, signal: cancellationToken });
      return response;
    }
    
    // Обновление статуса заказа
    public async updateOrderStatusAsync(body: OrderUpdateRequestDto, cancellationToken: AbortSignal): Promise<void> {
      await this.httpClient.put(`/admin/order/status`, body, { signal: cancellationToken });
    }

    // Удаление заказа
    public async deleteOrderAsync(orderId: string, cancellationToken: AbortSignal): Promise<void> {
      await this.httpClient.delete(`/admin/order/${orderId}`, { signal: cancellationToken });
    }

    //Обновление статуса платежа по заказу
    public async updatePaymentStatusByOrder(body: PaymentUpdateStatusRequestDto, cancellationToken: AbortSignal): Promise<void>{
      await this.httpClient.put(`/admin/payment/status`, body, { signal: cancellationToken });
    }


    //Работа с отзывами
    // Получение всех неподтвержденных отзывов
    public async getAllNotApprovedReviews(cancellationToken: AbortSignal): Promise<ReviewResponseDto[]> {
      return this.httpClient.get<ReviewResponseDto[]>('/admin/reviews', { signal: cancellationToken });
    }

    
    // Удаление отзыва
    public async deleteReview(id: string, cancellationToken: AbortSignal): Promise<void> {
      return this.httpClient.delete(`/admin/review/${id}`, { signal: cancellationToken });
    }

    // Подтверждение отзыва
    public async approveReview(id: string, cancellationToken: AbortSignal): Promise<void> {
      return this.httpClient.put(`/admin/review/${id}/approve`, null, { signal: cancellationToken });
    }


    //Работа с пользователями

    // Получение всех пользователей с фильтрацией, сортировкой и пагинацией
    public async getAllUsers(filterDto: FilterDto, cancellationToken: AbortSignal): Promise<PagedResponse<UserResponseDto>> {
      const params = new URLSearchParams();
      for (const [key, value] of Object.entries(filterDto)) {
        if (value !== undefined && value !== null) {
          params.append(key, String(value));
        }
      }
      const response = await this.httpClient.get<PagedResponse<UserResponseDto>>('/admin/users', { params, signal: cancellationToken });
      return response;
    }

    // Получение пользователя по ID
    public async getUserById(id: string, cancellationToken: AbortSignal): Promise<UserResponseDto> {
      const response = await this.httpClient.get<UserResponseDto>(`/admin/user/${id}`, { signal: cancellationToken });
      return response;
    }

    // Удаление пользователя по ID
    public async deleteUser(id: string, cancellationToken: AbortSignal): Promise<void> {
      await this.httpClient.delete(`/admin/user/${id}`, { signal: cancellationToken });
    }

    // Добавление нового пользователя
    public async addUser(body: UserAddRequestDto, cancellationToken: AbortSignal): Promise<void> {
      await this.httpClient.post('/admin/add_user', body, { signal: cancellationToken });
    }

    //Обновление пользователя
    public async updateUser(id: string, body: AdminUserUpdateRequestDto, cancellationToken: AbortSignal): Promise<UserResponseDto> {
      const response = await this.httpClient.post<UserResponseDto>(`/admin/update_user/${id}`, body, { signal: cancellationToken });
      return response;
    }

    //Работа с клиентами магазина
      // Получение клиента по ID
  public async getCustomerById(id: string, cancellationToken: AbortSignal): Promise<CustomerResponseDto> {
    const response = await this.httpClient.get<CustomerResponseDto>(`/admin/customer/${id}`, {signal: cancellationToken,});
    return response;
  }


    //  --  Статистика  --
    public async getStatistic(cancellationToken: AbortSignal): Promise<StatisticResponse> {
        const response = await this.httpClient.get<StatisticResponse>('/admin/statistic', { signal: cancellationToken });
        return response;
    }
}