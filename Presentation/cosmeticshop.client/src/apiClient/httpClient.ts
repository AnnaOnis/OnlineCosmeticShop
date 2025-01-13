// src/services/http-client.ts
import axios, { AxiosInstance, InternalAxiosRequestConfig, AxiosResponse} from 'axios';

class HttpClient {
  private instance: AxiosInstance;

  constructor(baseURL: string) {
    this.instance = axios.create({
      baseURL: baseURL,
      timeout: 10000,
    });

    // Добавляем интерцептор для добавления токена JWT в заголовок
    this.instance.interceptors.request.use(
      (config: InternalAxiosRequestConfig) => {
        const token = localStorage.getItem('jwtToken');
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      },
      (error) => {
        return Promise.reject(error);
      }
    );

    // Добавляем интерцептор для обработки ошибок
    this.instance.interceptors.response.use(
      (response: AxiosResponse) => {
        return response;
      },
      (error) => {
        if (error.response && error.response.status === 401) {
          // Обработка ошибки авторизации
          // Например, перенаправление на страницу логина
          window.location.href = '/login';
        }
        return Promise.reject(error);
      }
    );
  }

  public get<T = any>(url: string, config?: Partial<InternalAxiosRequestConfig>): Promise<T> {
    return this.instance.get(url, config as InternalAxiosRequestConfig).then((response) => response.data);
  }

  public post<T = any>(url: string, data?: any, config?: Partial<InternalAxiosRequestConfig>): Promise<T> {
    return this.instance.post(url, data, config as InternalAxiosRequestConfig).then((response) => response.data);
  }

  public put<T = any>(url: string, data?: any, config?: Partial<InternalAxiosRequestConfig>): Promise<T> {
    return this.instance.put(url, data, config as InternalAxiosRequestConfig).then((response) => response.data);
  }

  public delete<T = any>(url: string, config?: Partial<InternalAxiosRequestConfig>): Promise<T> {
    return this.instance.delete(url, config as InternalAxiosRequestConfig).then((response) => response.data);
  }
}

export default HttpClient;