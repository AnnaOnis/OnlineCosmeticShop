import HttpClient from '../httpClient';
import { ReviewResponseDto, ReviewCreateRequestDto } from '../models';

export class ReviewsService {
  private httpClient: HttpClient;

  constructor(basePath: string) {
    this.httpClient = new HttpClient(basePath);
  }

  // Получение всех неподтвержденных отзывов
  async getAllNotApprovedReviews(cancellationToken: AbortSignal): Promise<ReviewResponseDto[]> {
    return this.httpClient.get<ReviewResponseDto[]>('/reviews', { signal: cancellationToken });
  }

  // Получение подтвержденных отзывов по продукту
  async getApprovedReviewsByProduct(productId: string, cancellationToken: AbortSignal): Promise<ReviewResponseDto[]> {
    return this.httpClient.get<ReviewResponseDto[]>(`/reviews/approved/${productId}`, { signal: cancellationToken });
  }

  // Получение всех отзывов по продукту
  async getReviewsByProduct(productId: string, cancellationToken: AbortSignal): Promise<ReviewResponseDto[]> {
    return this.httpClient.get<ReviewResponseDto[]>(`/reviews/product/${productId}`, { signal: cancellationToken });
  }

  // Создание отзыва
  async createReview(reviewRequestDto: ReviewCreateRequestDto, cancellationToken: AbortSignal): Promise<ReviewResponseDto> {
    return this.httpClient.post<ReviewResponseDto>('/reviews', reviewRequestDto, { signal: cancellationToken });
  }

  // Удаление отзыва
  async deleteReview(id: string, cancellationToken: AbortSignal): Promise<void> {
    return this.httpClient.delete(`/reviews/${id}`, { signal: cancellationToken });
  }

  // Подтверждение отзыва
  async approveReview(id: string, cancellationToken: AbortSignal): Promise<void> {
    return this.httpClient.put(`/reviews/${id}/approve`, null, { signal: cancellationToken });
  }
}