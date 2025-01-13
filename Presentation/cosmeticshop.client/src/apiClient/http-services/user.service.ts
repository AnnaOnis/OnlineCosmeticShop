import HttpClient from '../httpClient';
import { PasswordResetRequestDto, UserAddRequestDto, UserUpdateRequestDto, FilterDto, UserResponseDto } from '../models';

export class UserService {
  private httpClient: HttpClient;

  constructor(basePath: string) {
    this.httpClient = new HttpClient(basePath);
  }

  // Получение всех пользователей с фильтрацией, сортировкой и пагинацией
  public async getAllUsers(filterDto: FilterDto, cancellationToken: AbortSignal): Promise<UserResponseDto[]> {
    const params = new URLSearchParams();
    for (const [key, value] of Object.entries(filterDto)) {
      if (value !== undefined && value !== null) {
        params.append(key, String(value));
      }
    }
    const response = await this.httpClient.get<UserResponseDto[]>('/user', { params, signal: cancellationToken });
    return response;
  }

  // Получение пользователя по ID
  public async getUserById(id: string, cancellationToken: AbortSignal): Promise<UserResponseDto> {
    const response = await this.httpClient.get<UserResponseDto>(`/user/${id}`, { signal: cancellationToken });
    return response;
  }

  // Удаление пользователя по ID
  public async deleteUser(id: string, cancellationToken: AbortSignal): Promise<void> {
    await this.httpClient.delete(`/user/${id}`, { signal: cancellationToken });
  }

  // Добавление нового пользователя
  public async addUser(userAddRequestDto: UserAddRequestDto, cancellationToken: AbortSignal): Promise<void> {
    await this.httpClient.post('/user/add_user', userAddRequestDto, { signal: cancellationToken });
  }

  // Обновление профиля пользователя
  public async updateUserProfile(userUpdateRequestDto: UserUpdateRequestDto, cancellationToken: AbortSignal): Promise<UserResponseDto> {
    const response = await this.httpClient.put<UserResponseDto>(`/user`, userUpdateRequestDto, { signal: cancellationToken });
    return response;
  }

  // Сброс пароля пользователя
  public async resetPassword(passwordResetRequestDto: PasswordResetRequestDto, cancellationToken: AbortSignal): Promise<void> {
    await this.httpClient.post(`/user/reset_password`, passwordResetRequestDto, { signal: cancellationToken });
  }

  // Получение текущего профиля пользователя
  public async getCurrentUserProfile(cancellationToken: AbortSignal): Promise<UserResponseDto> {
    const response = await this.httpClient.get<UserResponseDto>('/user/current', { signal: cancellationToken });
    return response;
  }
}