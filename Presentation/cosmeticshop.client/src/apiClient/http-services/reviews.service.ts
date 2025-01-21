import HttpClient from '../httpClient';
import { ReviewResponseDto, ReviewCreateRequestDto } from '../models';

export class ReviewsService {
  private httpClient: HttpClient;

  constructor(basePath: string) {
    this.httpClient = new HttpClient(basePath);
  }

  // Получение всех неподтвержденных отзывов
  public async getAllNotApprovedReviews(cancellationToken: AbortSignal): Promise<ReviewResponseDto[]> {
    return this.httpClient.get<ReviewResponseDto[]>('/reviews', { signal: cancellationToken });
  }

  // Получение подтвержденных отзывов по продукту
  public async getApprovedReviewsByProduct(productId: string, cancellationToken: AbortSignal): Promise<ReviewResponseDto[]> {
    return this.httpClient.get<ReviewResponseDto[]>(`/reviews/approved/${productId}`, { signal: cancellationToken });
  }

  // Получение всех отзывов по продукту
  public async getReviewsByProduct(productId: string, cancellationToken: AbortSignal): Promise<ReviewResponseDto[]> {
    return this.httpClient.get<ReviewResponseDto[]>(`/reviews/product/${productId}`, { signal: cancellationToken });
  }

  // Создание отзыва
  public async createReview(reviewRequestDto: ReviewCreateRequestDto, cancellationToken: AbortSignal): Promise<ReviewResponseDto> {
    return this.httpClient.post<ReviewResponseDto>('/reviews', reviewRequestDto, { signal: cancellationToken });
  }

  // Удаление отзыва
  public async deleteReview(id: string, cancellationToken: AbortSignal): Promise<void> {
    return this.httpClient.delete(`/reviews/${id}`, { signal: cancellationToken });
  }

  // Подтверждение отзыва
  public async approveReview(id: string, cancellationToken: AbortSignal): Promise<void> {
    return this.httpClient.put(`/reviews/${id}/approve`, null, { signal: cancellationToken });
  }
}