import HttpClient from '../httpClient';
import { StatisticResponse } from '../models/statistic-response';

export class AdminService{
    private httpClient: HttpClient;

    constructor(basePath: string) {
      this.httpClient = new HttpClient(basePath);
    }

    public async getStatistic(cancellationToken: AbortSignal): Promise<StatisticResponse> {
        const response = await this.httpClient.get<StatisticResponse>('/admin/statistic', { signal: cancellationToken });
        return response;
    }
}