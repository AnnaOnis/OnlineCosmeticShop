

/**
 * 
 * @export
 * @interface OrderItemResponseDto
 */
export interface OrderItemResponseDto {
    /**
     * @type {string}
     * @memberof OrderItemResponseDto
     */
    orderId: string;

    /**
     * @type {string}
     * @memberof OrderItemResponseDto
     */
    productId: string;

    /**
     * @type {string}
     * @memberof OrderItemResponseDto
     */
    productName: string;

    /**
     * @type {number}
     * @memberof OrderItemResponseDto
     */
    productPrice: number;

    /**
     * @type {number}
     * @memberof OrderItemResponseDto
     */
    quantity: number;
}