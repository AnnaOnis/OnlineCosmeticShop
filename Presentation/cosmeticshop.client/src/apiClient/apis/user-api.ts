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

import globalAxios, { AxiosResponse, AxiosInstance, AxiosRequestConfig } from 'axios';
import { Configuration } from '../configuration';
// Some imports not used depending on template conditions
// @ts-ignore
import { BASE_PATH, COLLECTION_FORMATS, RequestArgs, BaseAPI, RequiredError } from '../base';
import { PasswordResetRequestDto } from '../models';
import { UserAddRequestDto } from '../models';
import { UserResponseDto } from '../models';
import { UserUpdateRequestDto } from '../models';
/**
 * UserApi - axios parameter creator
 * @export
 */
export const UserApiAxiosParamCreator = function (configuration?: Configuration) {
    return {
        /**
         * 
         * @param {UserAddRequestDto} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiUserAddUserPost: async (body?: UserAddRequestDto, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/User/add_user`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'POST', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            localVarHeaderParameter['Content-Type'] = 'application/json';

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};
            const needsSerialization = (typeof body !== "string") || localVarRequestOptions.headers['Content-Type'] === 'application/json';
            localVarRequestOptions.data =  needsSerialization ? JSON.stringify(body !== undefined ? body : {}) : (body || "");

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiUserCurrentGet: async (options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/User/current`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'GET', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @param {string} [filter] 
         * @param {string} [sortField] 
         * @param {boolean} [sortOrder] 
         * @param {number} [pageNumber] 
         * @param {number} [pageSize] 
         * @param {string} [categoryId] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiUserGet: async (filter?: string, sortField?: string, sortOrder?: boolean, pageNumber?: number, pageSize?: number, categoryId?: string, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/User`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'GET', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            if (filter !== undefined) {
                localVarQueryParameter['Filter'] = filter;
            }

            if (sortField !== undefined) {
                localVarQueryParameter['SortField'] = sortField;
            }

            if (sortOrder !== undefined) {
                localVarQueryParameter['SortOrder'] = sortOrder;
            }

            if (pageNumber !== undefined) {
                localVarQueryParameter['PageNumber'] = pageNumber;
            }

            if (pageSize !== undefined) {
                localVarQueryParameter['PageSize'] = pageSize;
            }

            if (categoryId !== undefined) {
                localVarQueryParameter['CategoryId'] = categoryId;
            }

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @param {string} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiUserIdDelete: async (id: string, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            // verify required parameter 'id' is not null or undefined
            if (id === null || id === undefined) {
                throw new RequiredError('id','Required parameter id was null or undefined when calling apiUserIdDelete.');
            }
            const localVarPath = `/api/User/{id}`
                .replace(`{${"id"}}`, encodeURIComponent(String(id)));
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'DELETE', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @param {string} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiUserIdGet: async (id: string, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            // verify required parameter 'id' is not null or undefined
            if (id === null || id === undefined) {
                throw new RequiredError('id','Required parameter id was null or undefined when calling apiUserIdGet.');
            }
            const localVarPath = `/api/User/{id}`
                .replace(`{${"id"}}`, encodeURIComponent(String(id)));
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'GET', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @param {UserUpdateRequestDto} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiUserPut: async (body?: UserUpdateRequestDto, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/User`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'PUT', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            localVarHeaderParameter['Content-Type'] = 'application/json';

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};
            const needsSerialization = (typeof body !== "string") || localVarRequestOptions.headers['Content-Type'] === 'application/json';
            localVarRequestOptions.data =  needsSerialization ? JSON.stringify(body !== undefined ? body : {}) : (body || "");

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
        /**
         * 
         * @param {PasswordResetRequestDto} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        apiUserResetPasswordPost: async (body?: PasswordResetRequestDto, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
            const localVarPath = `/api/User/reset_password`;
            // use dummy base URL string because the URL constructor only accepts absolute URLs.
            const localVarUrlObj = new URL(localVarPath, 'https://example.com');
            let baseOptions;
            if (configuration) {
                baseOptions = configuration.baseOptions;
            }
            const localVarRequestOptions :AxiosRequestConfig = { method: 'POST', ...baseOptions, ...options};
            const localVarHeaderParameter = {} as any;
            const localVarQueryParameter = {} as any;

            localVarHeaderParameter['Content-Type'] = 'application/json';

            const query = new URLSearchParams(localVarUrlObj.search);
            for (const key in localVarQueryParameter) {
                query.set(key, localVarQueryParameter[key]);
            }
            for (const key in options.params) {
                query.set(key, options.params[key]);
            }
            localVarUrlObj.search = (new URLSearchParams(query)).toString();
            let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
            localVarRequestOptions.headers = {...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers};
            const needsSerialization = (typeof body !== "string") || localVarRequestOptions.headers['Content-Type'] === 'application/json';
            localVarRequestOptions.data =  needsSerialization ? JSON.stringify(body !== undefined ? body : {}) : (body || "");

            return {
                url: localVarUrlObj.pathname + localVarUrlObj.search + localVarUrlObj.hash,
                options: localVarRequestOptions,
            };
        },
    }
};

/**
 * UserApi - functional programming interface
 * @export
 */
export const UserApiFp = function(configuration?: Configuration) {
    return {
        /**
         * 
         * @param {UserAddRequestDto} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiUserAddUserPost(body?: UserAddRequestDto, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<void>>> {
            const localVarAxiosArgs = await UserApiAxiosParamCreator(configuration).apiUserAddUserPost(body, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
        /**
         * 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiUserCurrentGet(options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<UserResponseDto>>> {
            const localVarAxiosArgs = await UserApiAxiosParamCreator(configuration).apiUserCurrentGet(options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
        /**
         * 
         * @param {string} [filter] 
         * @param {string} [sortField] 
         * @param {boolean} [sortOrder] 
         * @param {number} [pageNumber] 
         * @param {number} [pageSize] 
         * @param {string} [categoryId] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiUserGet(filter?: string, sortField?: string, sortOrder?: boolean, pageNumber?: number, pageSize?: number, categoryId?: string, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<Array<UserResponseDto>>>> {
            const localVarAxiosArgs = await UserApiAxiosParamCreator(configuration).apiUserGet(filter, sortField, sortOrder, pageNumber, pageSize, categoryId, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
        /**
         * 
         * @param {string} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiUserIdDelete(id: string, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<void>>> {
            const localVarAxiosArgs = await UserApiAxiosParamCreator(configuration).apiUserIdDelete(id, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
        /**
         * 
         * @param {string} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiUserIdGet(id: string, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<UserResponseDto>>> {
            const localVarAxiosArgs = await UserApiAxiosParamCreator(configuration).apiUserIdGet(id, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
        /**
         * 
         * @param {UserUpdateRequestDto} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiUserPut(body?: UserUpdateRequestDto, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<UserResponseDto>>> {
            const localVarAxiosArgs = await UserApiAxiosParamCreator(configuration).apiUserPut(body, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
        /**
         * 
         * @param {PasswordResetRequestDto} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiUserResetPasswordPost(body?: PasswordResetRequestDto, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => Promise<AxiosResponse<void>>> {
            const localVarAxiosArgs = await UserApiAxiosParamCreator(configuration).apiUserResetPasswordPost(body, options);
            return (axios: AxiosInstance = globalAxios, basePath: string = BASE_PATH) => {
                const axiosRequestArgs :AxiosRequestConfig = {...localVarAxiosArgs.options, url: basePath + localVarAxiosArgs.url};
                return axios.request(axiosRequestArgs);
            };
        },
    }
};

/**
 * UserApi - factory interface
 * @export
 */
export const UserApiFactory = function (configuration?: Configuration, basePath?: string, axios?: AxiosInstance) {
    return {
        /**
         * 
         * @param {UserAddRequestDto} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiUserAddUserPost(body?: UserAddRequestDto, options?: AxiosRequestConfig): Promise<AxiosResponse<void>> {
            return UserApiFp(configuration).apiUserAddUserPost(body, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiUserCurrentGet(options?: AxiosRequestConfig): Promise<AxiosResponse<UserResponseDto>> {
            return UserApiFp(configuration).apiUserCurrentGet(options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @param {string} [filter] 
         * @param {string} [sortField] 
         * @param {boolean} [sortOrder] 
         * @param {number} [pageNumber] 
         * @param {number} [pageSize] 
         * @param {string} [categoryId] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiUserGet(filter?: string, sortField?: string, sortOrder?: boolean, pageNumber?: number, pageSize?: number, categoryId?: string, options?: AxiosRequestConfig): Promise<AxiosResponse<Array<UserResponseDto>>> {
            return UserApiFp(configuration).apiUserGet(filter, sortField, sortOrder, pageNumber, pageSize, categoryId, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @param {string} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiUserIdDelete(id: string, options?: AxiosRequestConfig): Promise<AxiosResponse<void>> {
            return UserApiFp(configuration).apiUserIdDelete(id, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @param {string} id 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiUserIdGet(id: string, options?: AxiosRequestConfig): Promise<AxiosResponse<UserResponseDto>> {
            return UserApiFp(configuration).apiUserIdGet(id, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @param {UserUpdateRequestDto} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiUserPut(body?: UserUpdateRequestDto, options?: AxiosRequestConfig): Promise<AxiosResponse<UserResponseDto>> {
            return UserApiFp(configuration).apiUserPut(body, options).then((request) => request(axios, basePath));
        },
        /**
         * 
         * @param {PasswordResetRequestDto} [body] 
         * @param {*} [options] Override http request option.
         * @throws {RequiredError}
         */
        async apiUserResetPasswordPost(body?: PasswordResetRequestDto, options?: AxiosRequestConfig): Promise<AxiosResponse<void>> {
            return UserApiFp(configuration).apiUserResetPasswordPost(body, options).then((request) => request(axios, basePath));
        },
    };
};

/**
 * UserApi - object-oriented interface
 * @export
 * @class UserApi
 * @extends {BaseAPI}
 */
export class UserApi extends BaseAPI {
    /**
     * 
     * @param {UserAddRequestDto} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof UserApi
     */
    public async apiUserAddUserPost(body?: UserAddRequestDto, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        return UserApiFp(this.configuration).apiUserAddUserPost(body, options).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof UserApi
     */
    public async apiUserCurrentGet(options?: AxiosRequestConfig) : Promise<AxiosResponse<UserResponseDto>> {
        return UserApiFp(this.configuration).apiUserCurrentGet(options).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @param {string} [filter] 
     * @param {string} [sortField] 
     * @param {boolean} [sortOrder] 
     * @param {number} [pageNumber] 
     * @param {number} [pageSize] 
     * @param {string} [categoryId] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof UserApi
     */
    public async apiUserGet(filter?: string, sortField?: string, sortOrder?: boolean, pageNumber?: number, pageSize?: number, categoryId?: string, options?: AxiosRequestConfig) : Promise<AxiosResponse<Array<UserResponseDto>>> {
        return UserApiFp(this.configuration).apiUserGet(filter, sortField, sortOrder, pageNumber, pageSize, categoryId, options).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @param {string} id 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof UserApi
     */
    public async apiUserIdDelete(id: string, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        return UserApiFp(this.configuration).apiUserIdDelete(id, options).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @param {string} id 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof UserApi
     */
    public async apiUserIdGet(id: string, options?: AxiosRequestConfig) : Promise<AxiosResponse<UserResponseDto>> {
        return UserApiFp(this.configuration).apiUserIdGet(id, options).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @param {UserUpdateRequestDto} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof UserApi
     */
    public async apiUserPut(body?: UserUpdateRequestDto, options?: AxiosRequestConfig) : Promise<AxiosResponse<UserResponseDto>> {
        return UserApiFp(this.configuration).apiUserPut(body, options).then((request) => request(this.axios, this.basePath));
    }
    /**
     * 
     * @param {PasswordResetRequestDto} [body] 
     * @param {*} [options] Override http request option.
     * @throws {RequiredError}
     * @memberof UserApi
     */
    public async apiUserResetPasswordPost(body?: PasswordResetRequestDto, options?: AxiosRequestConfig) : Promise<AxiosResponse<void>> {
        return UserApiFp(this.configuration).apiUserResetPasswordPost(body, options).then((request) => request(this.axios, this.basePath));
    }
}
