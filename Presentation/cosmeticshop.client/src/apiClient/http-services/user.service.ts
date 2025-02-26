import HttpClient from '../httpClient';
import { PasswordResetRequestDto, UserUpdateRequestDto, UserResponseDto, } from '../models';

export class UserService {
  private httpClient: HttpClient;

  constructor(basePath: string) {
    this.httpClient = new HttpClient(basePath);
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