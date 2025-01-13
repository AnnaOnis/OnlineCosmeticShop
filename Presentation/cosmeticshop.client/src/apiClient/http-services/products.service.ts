import HttpClient from '../httpClient';
import { ProductRequestDto, ProductResponseDto, FilterDto } from '../models';

export class ProductsService {
  private httpClient: HttpClient;

  constructor(basePath: string) {
    this.httpClient = new HttpClient(basePath);
  }

  // Получение списка продуктов с фильтрацией
  public async getProducts(filterDto: FilterDto, cancellationToken: AbortSignal): Promise<ProductResponseDto[]> {
    const params = new URLSearchParams();
    for (const [key, value] of Object.entries(filterDto)) {
      if (value !== undefined && value !== null) {
        params.append(key, String(value));
      }
    }
    return await this.httpClient.get<ProductResponseDto[]>(`/products`, { params, signal: cancellationToken });
  }

  // Создание нового продукта
  public async createProduct(productRequestDto: ProductRequestDto, cancellationToken: AbortSignal): Promise<ProductResponseDto> {
    return await this.httpClient.post<ProductResponseDto>('/products', productRequestDto, { signal: cancellationToken });
  }

  // Получение продукта по ID
  public async getProductById(id: string, cancellationToken: AbortSignal): Promise<ProductResponseDto> {
    return await this.httpClient.get<ProductResponseDto>(`/products/${id}`, { signal: cancellationToken });
  }

  // Обновление продукта по ID
  public async updateProduct(id: string, productRequestDto: ProductRequestDto, cancellationToken: AbortSignal): Promise<ProductResponseDto> {
    return await this.httpClient.put<ProductResponseDto>(`/products/${id}`, productRequestDto, { signal: cancellationToken });
  }

  // Удаление продукта по ID
  public async deleteProduct(id: string, cancellationToken: AbortSignal): Promise<void> {
    await this.httpClient.delete(`/products/${id}`, { signal: cancellationToken });
  }
}