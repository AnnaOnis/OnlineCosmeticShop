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
 * @interface CustomerRegisterRequestDto
 */
export interface CustomerRegisterRequestDto {

    /**
     * @type {string}
     * @memberof CustomerRegisterRequestDto
     */
    firstName: string;

    /**
     * @type {string}
     * @memberof CustomerRegisterRequestDto
     */
    lastName: string;

    /**
     * @type {string}
     * @memberof CustomerRegisterRequestDto
     */
    email: string;

    /**
     * @type {string}
     * @memberof CustomerRegisterRequestDto
     */
    phoneNumber?: string | null;

    /**
     * @type {string}
     * @memberof CustomerRegisterRequestDto
     */
    shippingAddress?: string | null;

    /**
     * @type {string}
     * @memberof CustomerRegisterRequestDto
     */
    password: string;

    /**
     * @type {string}
     * @memberof CustomerRegisterRequestDto
     */
    confirmedPassword: string;
}
