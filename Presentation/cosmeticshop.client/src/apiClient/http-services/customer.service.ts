import HttpClient from '../httpClient';
import { CustomerUpdateRequestDto, CustomerResponseDto, FilterDto } from '../models';

export class CustomerService {
  private httpClient: HttpClient;

  constructor(basePath: string) {
    this.httpClient = new HttpClient(basePath);
  }

  // Получение всех клиентов с фильтрацией, сортировкой и пагинацией
  public async getAllCustomers(filterDto: FilterDto, cancellationToken: AbortSignal): Promise<CustomerResponseDto[]> {
    const params = new URLSearchParams();
    for (const [key, value] of Object.entries(filterDto)) {
      if (value !== undefined && value !== null) {
        params.append(key, String(value));
      }
    }
    const response = await this.httpClient.get<CustomerResponseDto[]>('/customer', {params, signal: cancellationToken,});
    return response;
  }

  // Получение клиента по ID
  public async getCustomerById(id: string, cancellationToken: AbortSignal): Promise<CustomerResponseDto> {
    const response = await this.httpClient.get<CustomerResponseDto>(`/customer/${id}`, {signal: cancellationToken,});
    return response;
  }

  // Удаление клиента по ID
  public async deleteCustomer(id: string, cancellationToken: AbortSignal): Promise<void> {
    await this.httpClient.delete(`/customer/${id}`, {signal: cancellationToken,});
  }

  // Получение текущего профиля клиента
  public async getCurrentCustomerProfile(cancellationToken: AbortSignal): Promise<CustomerResponseDto> {
    const response = await this.httpClient.get<CustomerResponseDto>('/customer/current', {signal: cancellationToken,});
    return response;
  }

    // Обновление профиля клиента
  public async updateCustomerProfile(
    email: string,
    firstName: string,
    lastName: string,
    phoneNumber: string,
    shippingAddress: string,
    cancellationToken: AbortSignal
  ): Promise<CustomerResponseDto> {
    const requestDto: CustomerUpdateRequestDto = {
      email,
      firstName,
      lastName,
      phoneNumber,
      shippingAddress,
    };
    const response = await this.httpClient.put<CustomerResponseDto>(`/customer`, requestDto, {signal: cancellationToken,});
    return response;
  }

  // Сброс пароля клиента
  public async resetPassword(newPassword: string, cancellationToken: AbortSignal): Promise<void> {
    const requestDto = { newPassword };
    await this.httpClient.post(`/customer`, requestDto, {signal: cancellationToken,});
  }


}