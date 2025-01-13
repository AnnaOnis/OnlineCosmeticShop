/**
 * 
 *
 * @export
 * @interface PagedResponse
 * @template T
 */
export interface PagedResponse<T> {

    /**
     * Список элементов типа T
     * @type {Array<T>}
     * @memberof PagedResponse
     */
    items: Array<T>;

    /**
     * Общее количество элементов
     * @type {number}
     * @memberof PagedResponse
     */
    totalItems: number;
}