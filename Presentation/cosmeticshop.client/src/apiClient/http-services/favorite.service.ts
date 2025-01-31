import HttpClient from '../httpClient';
import { FavoriteResponseDto, FavoriteRequestDto, ProductResponseDto, PagedResponse} from '../models'

export class FavoriteService {
  private httpClient: HttpClient;

  constructor(basePath: string) {
    this.httpClient = new HttpClient(basePath);
  }

  // Получение избранных продуктов по ID клиента
  public async getAllFavoritesByCustomerId(customerId: string, cancellationToken: AbortSignal): Promise<ProductResponseDto[]> {
    const url = `/favorites/all/customer/${customerId}`;
    return this.httpClient.get<ProductResponseDto[]>( url, { signal: cancellationToken });
  }

  // Получение избранных продуктов по ID клиента с пагинацией
  public async getFavoritesByCustomerIdPaginations(customerId: string, page: number, pageSize: number, cancellationToken: AbortSignal): Promise<PagedResponse<ProductResponseDto>> {
    const url = `/favorites/customer/${customerId}?page=${page}&pageSize=${pageSize}`;
    return this.httpClient.get<PagedResponse<ProductResponseDto>>( url, { signal: cancellationToken });
  }

  // Получение избранных продуктов по ID продукта
  public async getFavoritesByProductId(productId: string, page: number, pageSize: number, cancellationToken: AbortSignal): Promise<FavoriteResponseDto[]> {
    return this.httpClient.get<FavoriteResponseDto[]>(`/favorites/product/${productId}?page=${page}&pageSize=${pageSize}`, { signal: cancellationToken });
  }

  // Добавление продукта в избранное
  public async addToFavorites(requestDto: FavoriteRequestDto, cancellationToken: AbortSignal): Promise<void> {
    return this.httpClient.post<void>('/favorites', requestDto, { signal: cancellationToken });
  }

  // Удаление продукта из избранного
  public async removeFromFavorites(productId: string, customerId: string, cancellationToken: AbortSignal): Promise<void> {
    return this.httpClient.delete<void>(`/favorites?customerId=${customerId}&productId=${productId}`, { signal: cancellationToken });
  }
}