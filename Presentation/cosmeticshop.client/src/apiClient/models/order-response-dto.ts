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

import { OrderItemResponseDto } from './order-item-response-dto';
import { OrderStatus } from './order-status';
import { PaymentMethod } from './payment-method';
import { ShippingMethod } from './shipping-method';
 /**
 * 
 *
 * @export
 * @interface OrderResponseDto
 */
export interface OrderResponseDto {

    /**
     * @type {string}
     * @memberof OrderResponseDto
     */
    id?: string;

    /**
     * @type {string}
     * @memberof OrderResponseDto
     */
    customerId: string;

    /**
     * @type {Date}
     * @memberof OrderResponseDto
     */
    orderDate: Date;

    /**
     * @type {OrderStatus}
     * @memberof OrderResponseDto
     */
    status: OrderStatus;

    /**
     * @type {number}
     * @memberof OrderResponseDto
     */
    totalQuantity?: number;

    /**
     * @type {number}
     * @memberof OrderResponseDto
     */
    totalAmount?: number;

    /**
     * @type {ShippingMethod}
     * @memberof OrderResponseDto
     */
    orderShippingMethod: ShippingMethod;

    /**
     * @type {PaymentMethod}
     * @memberof OrderResponseDto
     */
    orderPaymentMethod: PaymentMethod;

    /**
     * @type {Array<OrderItemResponseDto>}
     * @memberof CartResponseDto
     */
    orderItems: Array<OrderItemResponseDto>;
}
