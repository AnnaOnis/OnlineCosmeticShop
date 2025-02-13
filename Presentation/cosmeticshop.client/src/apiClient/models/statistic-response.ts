/**
 * 
 *
 * @export
 * @interface StatisticResponse
 */
export interface StatisticResponse {

    /**
     * @type {number}
     * @memberof StatisticResponse
     */
    orderCount: number;

    /**
     * @type {number}
     * @memberof StatisticResponse
     */
    totalRevenue: number;

    /**
     * @type {number}
     * @memberof StatisticResponse
     */
    newCustomerCount: number;

    /**
     * @type {number}
     * @memberof StatisticResponse
     */
    approvedReviewCount: number;
}
