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
 * @interface ReviewResponseDto
 */
export interface ReviewResponseDto {

    /**
     * @type {string}
     * @memberof ReviewResponseDto
     */
    id?: string;

    /**
     * @type {string}
     * @memberof ReviewResponseDto
     */
    productId?: string;

    /**
     * @type {string}
     * @memberof ReviewResponseDto
     */
    customerId?: string;

        /**
     * @type {string}
     * @memberof ReviewResponseDto
     */
    customerName?: string;

    /**
     * @type {string}
     * @memberof ReviewResponseDto
     */
    reviewText?: string | null;

    /**
     * @type {number}
     * @memberof ReviewResponseDto
     */
    rating?: number;

    /**
     * @type {Date}
     * @memberof ReviewResponseDto
     */
    reviewDate: Date;

    /**
     * @type {boolean}
     * @memberof ReviewResponseDto
     */
    isApproved?: boolean;
}
