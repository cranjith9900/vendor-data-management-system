import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { InventorySummary, TopVendor, TransactionTrend } from '../models/models';

@Injectable({ providedIn: 'root' })
export class ReportService {
  private baseUrl = `${environment.apiUrl}/reports`;

  constructor(private http: HttpClient) {}

  getInventorySummary(): Observable<InventorySummary[]> {
    return this.http.get<InventorySummary[]>(`${this.baseUrl}/inventory-summary`);
  }

  getTransactionTrend(days = 30): Observable<TransactionTrend[]> {
    return this.http.get<TransactionTrend[]>(`${this.baseUrl}/transaction-trend`, {
      params: { days: String(days) }
    });
  }

  getTopVendors(take = 5): Observable<TopVendor[]> {
    return this.http.get<TopVendor[]>(`${this.baseUrl}/top-vendors`, {
      params: { take: String(take) }
    });
  }
}
