import HttpClient from '../httpClient';
import { CategoryRequestDto, CategoryResponseDto } from '../models';

export class CategoryService {
  private httpClient: HttpClient;

  constructor(basePath: string) {
    this.httpClient = new HttpClient(basePath);
  }

  // Получение всех категорий
  public async getAllCategories(cancellationToken: AbortSignal): Promise<CategoryResponseDto[]> {
    return this.httpClient.get<CategoryResponseDto[]>('/category', { signal: cancellationToken });
  }

  // Создание новой категории
  public async createCategory(category: CategoryRequestDto, cancellationToken: AbortSignal): Promise<CategoryResponseDto> {
    return this.httpClient.post<CategoryResponseDto>('/category', category, { signal: cancellationToken });
  }

  // Получение категории по ID
  public async getCategoryById(id: string, cancellationToken: AbortSignal): Promise<CategoryResponseDto> {
    return this.httpClient.get<CategoryResponseDto>(`/category/${id}`, { signal: cancellationToken });
  }

  // Обновление категории по ID
  public async updateCategory(id: string, category: CategoryRequestDto, cancellationToken: AbortSignal): Promise<CategoryResponseDto> {
    return this.httpClient.put<CategoryResponseDto>(`/category/${id}`, category, { signal: cancellationToken });
  }

  // Удаление категории по ID
  public async deleteCategory(id: string, cancellationToken: AbortSignal): Promise<void> {
    return this.httpClient.delete<void>(`/category/${id}`, { signal: cancellationToken });
  }
}