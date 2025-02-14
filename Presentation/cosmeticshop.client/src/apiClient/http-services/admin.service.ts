import HttpClient from '../httpClient';
import { ProductRequestDto, ProductResponseDto } from '../models';
import { StatisticResponse } from '../models/statistic-response';

export class AdminService{
    private httpClient: HttpClient;

    constructor(basePath: string) {
      this.httpClient = new HttpClient(basePath);
    }
    //  --  Работа с товарами  --
      // Создание нового продукта
    public async createProduct(productRequestDto: ProductRequestDto, cancellationToken: AbortSignal): Promise<ProductResponseDto> {
      return await this.httpClient.post<ProductResponseDto>('/admin/product/create', productRequestDto, { signal: cancellationToken });
    }
    // Обновление продукта по ID
    public async updateProduct(id: string, productRequestDto: ProductRequestDto, cancellationToken: AbortSignal): Promise<ProductResponseDto> {
      return await this.httpClient.put<ProductResponseDto>(`/admin/product/${id}`, productRequestDto, { signal: cancellationToken });
    }
    
    // Удаление продукта по ID
    public async deleteProduct(id: string, cancellationToken: AbortSignal): Promise<void> {
      await this.httpClient.delete(`/admin/product/${id}`, { signal: cancellationToken });
    }

    //  --  Статистика  --
    public async getStatistic(cancellationToken: AbortSignal): Promise<StatisticResponse> {
        const response = await this.httpClient.get<StatisticResponse>('/admin/statistic', { signal: cancellationToken });
        return response;
    }
}