 /**
 * 
 *
 * @export
 * @interface FilterDto
 */
 export interface FilterDto {

    /**
     * @type {string}
     * @memberof FilterDto
     */
    filter: string;

    /**
     * @type {string}
     * @memberof FilterDto
     */
    sortField: string;

    /**
     * @type {boolean}
     * @memberof FilterDto
     */
    sortOrder: boolean

    /**
     * @type {number}
     * @memberof FilterDto
     */
    pageNumber: number

    /**
     * @type {number}
     * @memberof FilterDto
     */
    pageSize: number   
    
    /**
    * @type {string}
    * @memberof FilterDto
    */
    categoryId?: string | null;
}