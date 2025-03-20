import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book } from '../models/book.model';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AnalyticsService {
  private apiUrl = `${environment.apiUrl}/Analytics`;

  constructor(private http: HttpClient) { }

  getBookCount(): Observable<{ count: number }> {
    return this.http.get<{ count: number }>(`${this.apiUrl}/book-count`);
  }

  getMostBorrowedBook(): Observable<{ book: Book; borrowCount: number }> {
    return this.http.get<{ book: Book; borrowCount: number }>(`${this.apiUrl}/most-borrowed`);
  }

  getTop10BorrowedBooks(): Observable<{ topBooks: { book: Book; borrowCount: number }[] }> {
    return this.http.get<{ topBooks: { book: Book; borrowCount: number }[] }>(`${this.apiUrl}/top-10-borrowed`);
  }
}