import HttpClient from '../httpClient';
import { ProductResponseDto, FilterDto, PagedResponse } from '../models';

export class ProductsService {
  private httpClient: HttpClient;

  constructor(basePath: string) {
    this.httpClient = new HttpClient(basePath);
  }

  // Получение списка продуктов с фильтрацией
  public async getProducts(filterDto: FilterDto, cancellationToken: AbortSignal): Promise<PagedResponse<ProductResponseDto>> {
    const params = new URLSearchParams();
    for (const [key, value] of Object.entries(filterDto)) {
      if (value !== undefined && value !== null) {
        params.append(key, String(value));
      }
    }
    return await this.httpClient.get<PagedResponse<ProductResponseDto>>(`/products`, { params, signal: cancellationToken });
  }

  // Получение продукта по ID
  public async getProductById(id: string, cancellationToken: AbortSignal): Promise<ProductResponseDto> {
    return await this.httpClient.get<ProductResponseDto>(`/products/${id}`, { signal: cancellationToken });
  }
}