// src/services/auth.service.ts
import HttpClient from '../httpClient';
import { CustomerRegisterRequestDto, 
        CustomerResponseDto, 
        LoginRequest, 
        LogoutRequest, 
        UserResponseDto } from '../models';

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

  public async login(body: LoginRequest, cancellationToken: AbortSignal): Promise<CustomerResponseDto | UserResponseDto> {
    const response = await this.httpClient.post('/auth/login', body, { signal: cancellationToken });
    localStorage.setItem('jwtToken', response.token); // Сохраняем токен в localStorage
    return response;
  }

  public async logout(body: LogoutRequest, cancellationToken: AbortSignal): Promise<void> {
    await this.httpClient.post('/auth/logout', body, { signal: cancellationToken });
    localStorage.removeItem('jwtToken'); // Удаляем токен из localStorage
  }

}