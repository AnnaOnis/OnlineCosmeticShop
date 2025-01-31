 /**
 * 
 *
 * @export
 * @interface FavoriteResponseDto
 */
 export interface FavoriteResponseDto {

    /**
     * @type {string}
     * @memberof FavoriteResponseDto
     */
    id: string;

    /**
     * @type {string}
     * @memberof FavoriteResponseDto
     */
    productId: string;

    /**
     * @type {string}
     * @memberof FavoriteResponseDto
     */
    customerId: string;

    /**
     * @type {string}
     * @memberof FavoriteResponseDto
     */
    productName: string;

    /**
     * @type {string}
     * @memberof FavoriteResponseDto
     */
    productDescription: string;

    /**
     * @type {number}
     * @memberof FavoriteResponseDto
     */
    productRating: number;

    /**
     * @type {number}
     * @memberof FavoriteResponseDto
     */
    productPrice: number;

    /**
     * @type {string}
     * @memberof FavoriteResponseDto
     */
    productImageUrl: string;
}

