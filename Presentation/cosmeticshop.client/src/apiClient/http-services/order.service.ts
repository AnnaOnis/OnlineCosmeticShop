import HttpClient from '../httpClient';
import { OrderCreateRequestDto, OrderResponseDto } from '../models';

export class OrderService {
  private httpClient: HttpClient;

  constructor(basePath: string) {
    this.httpClient = new HttpClient(basePath);
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
}