import HttpClient from '../httpClient';
import { OrderCreateRequestDto, OrderUpdateRequestDto, OrderResponseDto, FilterDto, PagedResponse } from '../models';

export class OrderService {
  private httpClient: HttpClient;

  constructor(basePath: string) {
    this.httpClient = new HttpClient(basePath);
  }

 // Получение всех заказов с фильтрацией, сортировкой и пагинацией
 async getAllOrdersAsync(filterDto: FilterDto, cancellationToken: AbortSignal): Promise<PagedResponse<OrderResponseDto>> {
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
async getOrderDetailsAsync(id: string, cancellationToken: AbortSignal): Promise<OrderResponseDto> {
  const response = await this.httpClient.get<OrderResponseDto>(`/order/${id}`, { signal: cancellationToken });
  return response;
}

// Получение заказов конкретного клиента
async getCustomerOrdersAsync(cancellationToken: AbortSignal): Promise<OrderResponseDto[]> {
  const response = await this.httpClient.get<OrderResponseDto[]>('/order/customer', { signal: cancellationToken });
  return response;
}

// Создание нового заказа
async createOrderAsync(orderCreateRequestDto: OrderCreateRequestDto, cancellationToken: AbortSignal): Promise<OrderResponseDto> {
  const response = await this.httpClient.post<OrderResponseDto>('/order', orderCreateRequestDto, { signal: cancellationToken });
  return response;
}

// Обновление статуса заказа
async updateOrderStatusAsync(orderUpdateRequestDto: OrderUpdateRequestDto, cancellationToken: AbortSignal): Promise<void> {
  await this.httpClient.put(`/order`, orderUpdateRequestDto, { signal: cancellationToken });
}

// Удаление заказа
async deleteOrderAsync(orderId: string, cancellationToken: AbortSignal): Promise<void> {
  await this.httpClient.delete(`/order/${orderId}`, { signal: cancellationToken });
}
}