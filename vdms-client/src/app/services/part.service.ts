import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Part } from '../models/models';

@Injectable({ providedIn: 'root' })
export class PartService {
  private baseUrl = `${environment.apiUrl}/parts`;

  constructor(private http: HttpClient) {}

  getAll(search?: string, category?: string): Observable<Part[]> {
    const params: Record<string, string> = {};
    if (search) params['search'] = search;
    if (category) params['category'] = category;
    return this.http.get<Part[]>(this.baseUrl, { params });
  }

  getById(id: number): Observable<Part> {
    return this.http.get<Part>(`${this.baseUrl}/${id}`);
  }

  create(part: Partial<Part>): Observable<Part> {
    return this.http.post<Part>(this.baseUrl, part);
  }

  update(id: number, part: Partial<Part>): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, { ...part, id });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
