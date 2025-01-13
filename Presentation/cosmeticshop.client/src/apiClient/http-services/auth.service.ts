// src/services/auth.service.ts
import HttpClient from '../httpClient';
import { CustomerRegisterRequestDto, CustomerResponseDto, UserResponseDto } from '../models';

export class AuthService {
  private httpClient: HttpClient;

  constructor(basePath: string) {
    this.httpClient = new HttpClient(basePath);
  }

  public async register(body: CustomerRegisterRequestDto, cancellationToken: AbortSignal): Promise<CustomerResponseDto> {
    const response = await this.httpClient.post('/auth/register', body, { signal: cancellationToken });
    localStorage.setItem('jwtToken', response.token); // Сохраняем токен в localStorage
    return response;
  }

  public async login(body: any, cancellationToken: AbortSignal): Promise<CustomerResponseDto | UserResponseDto> {
    const response = await this.httpClient.post('/auth/login', body, { signal: cancellationToken });
    localStorage.setItem('jwtToken', response.token); // Сохраняем токен в localStorage
    return response;
  }

  public async logout(cancellationToken: AbortSignal): Promise<void> {
    await this.httpClient.post('/auth/logout', { signal: cancellationToken });
    localStorage.removeItem('jwtToken'); // Удаляем токен из localStorage
  }

}