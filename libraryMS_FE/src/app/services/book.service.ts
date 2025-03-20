import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book } from '../models/book.model';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BookService {
  private apiUrl = `${environment.apiUrl}/Books`;

  constructor(private http: HttpClient) { }

  getBooks(): Observable<{ books: Book[] }> {
    return this.http.get<{ books: Book[] }>(this.apiUrl);
  }

  addBook(book: Omit<Book, 'id'>): Observable<any> {
    return this.http.post<any>(this.apiUrl, book);
  }

  updateBook(id: string, book: Book): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, book);
  }

  deleteBook(id: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }

  importBooks(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<any>(`${this.apiUrl}/import`, formData);
  }
}