/* tslint:disable */
/* eslint-disable */
/**
 * CosmeticShop.WebAPI
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.0
 * 
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */

 /**
 * 
 *
 * @export
 * @interface CartItemResponseDto
 */
export interface CartItemResponseDto {

    /**
     * @type {string}
     * @memberof CartItemResponseDto
     */
    id: string;

    /**
     * @type {string}
     * @memberof CartItemResponseDto
     */
    cartId: string;

    /**
     * @type {string}
     * @memberof CartItemResponseDto
     */
    productId: string;

    /**
     * @type {string}
     * @memberof CartItemResponseDto
     */
    productName: string;

    /**
     * @type {number}
     * @memberof CartItemResponseDto
     */
    productPrice: number;

    /**
     * @type {number}
     * @memberof CartItemResponseDto
     */
    quantity: number;

    /**
     * @type {string}
     * @memberof CartItemResponseDto
     */
    productImageUrl: string;
}
