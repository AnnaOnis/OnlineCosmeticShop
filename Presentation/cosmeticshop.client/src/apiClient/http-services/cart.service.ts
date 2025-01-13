import HttpClient from '../httpClient';
import { CartItemRequestDto, CartResponseDto } from '../models';

export class CartService {
    private httpClient: HttpClient;

  constructor(basePath: string) {
    this.httpClient = new HttpClient(basePath)
  }

  public async getCart(cancellationToken: AbortSignal): Promise<CartResponseDto> {
    return this.httpClient.get<CartResponseDto>('/cart', { signal: cancellationToken });
  }

  public async addItemToCart(body: CartItemRequestDto, cancellationToken: AbortSignal): Promise<CartResponseDto> {
    return this.httpClient.post<CartResponseDto>('/cart/add_item', body, { signal: cancellationToken });
  }

  public async updateItemQuantity(body: CartItemRequestDto, cancellationToken: AbortSignal): Promise<CartResponseDto> {
    return this.httpClient.post<CartResponseDto>('/cart/update_item_quantity', body, { signal: cancellationToken });
  }

  public async removeItemFromCart(productId: string, cancellationToken: AbortSignal): Promise<void> {
    return this.httpClient.delete<void>(`/cart/${productId}`, { signal: cancellationToken });
  }

  public async clearCart(cancellationToken: AbortSignal): Promise<void> {
    return this.httpClient.delete<void>('/cart', { signal: cancellationToken });
  }
}