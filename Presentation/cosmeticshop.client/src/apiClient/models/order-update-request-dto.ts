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

import { OrderStatus } from './order-status';
 /**
 * 
 *
 * @export
 * @interface OrderUpdateRequestDto
 */
export interface OrderUpdateRequestDto {

    /**
     * @type {string}
     * @memberof OrderUpdateRequestDto
     */
    id: string;

    /**
     * @type {OrderStatus}
     * @memberof OrderUpdateRequestDto
     */
    newStatus: OrderStatus;
}
