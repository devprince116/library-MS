import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BorrowRecord } from '../models/borrow.model';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BorrowService {
  private apiUrl = `${environment.apiUrl}/Borrow`;

  constructor(private http: HttpClient) { }

  issueBook(userId: string, bookId: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/issue`, null, { params: { userId, bookId } });
  }

  returnBook(borrowRecordId: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/return`, null, { params: { borrowRecordId } });
  }

  getUserBorrowRecords(userId: string): Observable<{ records: BorrowRecord[] }> {
    return this.http.get<{ records: BorrowRecord[] }>(`${this.apiUrl}/user/${userId}`);
  }

  getAllBorrowRecords(): Observable<{ records: BorrowRecord[] }> {
    return this.http.get<{ records: BorrowRecord[] }>(`${this.apiUrl}`);
  }
}