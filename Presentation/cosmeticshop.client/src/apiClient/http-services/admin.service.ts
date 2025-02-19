import HttpClient from '../httpClient';
import { FilterDto, 
         OrderResponseDto, 
         OrderUpdateRequestDto,
         PagedResponse, 
         ProductRequestDto, 
         ProductResponseDto, 
         ReviewResponseDto} from '../models';
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


    //  --  Статистика  --
    public async getStatistic(cancellationToken: AbortSignal): Promise<StatisticResponse> {
        const response = await this.httpClient.get<StatisticResponse>('/admin/statistic', { signal: cancellationToken });
        return response;
    }
}