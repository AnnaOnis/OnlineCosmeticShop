import HttpClient from '../httpClient';
import { OrderCreateRequestDto, OrderUpdateRequestDto, OrderResponseDto, FilterDto, PagedResponse } from '../models';

export class OrderService {
  private httpClient: HttpClient;

  constructor(basePath: string) {
    this.httpClient = new HttpClient(basePath);
  }

 // Получение всех заказов с фильтрацией, сортировкой и пагинацией
  public async getAllOrdersAsync(filterDto: FilterDto, cancellationToken: AbortSignal): Promise<PagedResponse<OrderResponseDto>> {
    const params = new URLSearchParams();
    for (const [key, value] of Object.entries(filterDto)) {
      if (value !== undefined && value !== null) {
        params.append(key, String(value));
      }
    }
    const response = await this.httpClient.get<PagedResponse<OrderResponseDto>>('/order', { params, signal: cancellationToken });
    return response;
  }

  // Получение деталей заказа по ID
  public async getOrderDetailsAsync(id: string, cancellationToken: AbortSignal): Promise<OrderResponseDto> {
    const response = await this.httpClient.get<OrderResponseDto>(`/order/${id}`, { signal: cancellationToken });
    return response;
  }

  // Получение заказов конкретного клиента
  public async getCustomerOrdersAsync(cancellationToken: AbortSignal): Promise<OrderResponseDto[]> {
    const response = await this.httpClient.get<OrderResponseDto[]>('/order/customer', { signal: cancellationToken });
    return response;
  }

  // Создание нового заказа
  public async createOrderAsync(body: OrderCreateRequestDto, cancellationToken: AbortSignal): Promise<OrderResponseDto> {
    console.log("Sending OrderCreateRequestDto:");
    console.log(body);
    const response = await this.httpClient.post<OrderResponseDto>('/order', body, { signal: cancellationToken });
    return response;
  }

  //Оплата заказа
  public async orderPaymentProcessing(orderId: string, cancellationToken: AbortSignal): Promise<OrderResponseDto> {
    const response = await this.httpClient.post<OrderResponseDto>(`/order/pay/${orderId}`, { signal: cancellationToken });
    return response;
  }

  // Обновление статуса заказа
  public async updateOrderStatusAsync(body: OrderUpdateRequestDto, cancellationToken: AbortSignal): Promise<void> {
    await this.httpClient.put(`/order`, body, { signal: cancellationToken });
  }

  // Удаление заказа
  public async deleteOrderAsync(orderId: string, cancellationToken: AbortSignal): Promise<void> {
    await this.httpClient.delete(`/order/${orderId}`, { signal: cancellationToken });
  }
}