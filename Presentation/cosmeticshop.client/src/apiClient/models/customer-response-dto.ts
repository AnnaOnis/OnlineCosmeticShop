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
 * @interface CustomerResponseDto
 */
export interface CustomerResponseDto {

    /**
     * @type {string}
     * @memberof CustomerResponseDto
     */
    token?: string | null;

    /**
     * @type {string}
     * @memberof CustomerResponseDto
     */
    id: string;

    /**
     * @type {string}
     * @memberof CustomerResponseDto
     */
    firstName: string;

    /**
     * @type {string}
     * @memberof CustomerResponseDto
     */
    lastName: string;

    /**
     * @type {string}
     * @memberof CustomerResponseDto
     */
    email: string;

    /**
     * @type {string}
     * @memberof CustomerResponseDto
     */
    phoneNumber?: string | null;

    /**
     * @type {string}
     * @memberof CustomerResponseDto
     */
    shippingAddress?: string | null;
}
