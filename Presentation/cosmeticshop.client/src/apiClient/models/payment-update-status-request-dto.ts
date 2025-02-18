import { OrderStatus } from './order-status';
 /**
 * 
 *
 * @export
 * @interface PaymentUpdateStatusRequestDto
 */
export interface PaymentUpdateStatusRequestDto {

    /**
     * @type {string}
     * @memberof PaymentUpdateStatusRequestDto
     */
    orderId: string;

    /**
     * @type {OrderStatus}
     * @memberof PaymentUpdateStatusRequestDto
     */
    newPaymentStatus: OrderStatus;
}