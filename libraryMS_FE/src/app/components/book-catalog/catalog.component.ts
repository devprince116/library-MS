import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { BookService } from '../../services/book.service';
import { BorrowService } from '../../services/borrow.service';
import { AuthService } from '../../services/auth.service';
import { Book } from '../../models/book.model';
import { FormsModule } from '@angular/forms';
import { LoaderComponent } from "../loader/loader.component";
import { slideInOut } from '../../assets/animations';

@Component({
  selector: 'app-book-catalog',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatFormFieldModule, MatInputModule, MatButtonModule, FormsModule, LoaderComponent],
  templateUrl: "./catalog.component.html",
  styles: [
    `
      .catalog-container {
        padding: 20px;
      }
      mat-table {
        width: 100%;
        margin-top: 20px;
      }
      mat-form-field {
        width: 100%;
      }
    `,
  ],
  animations: [slideInOut],
})
export class BookCatalogComponent implements OnInit {
  books: Book[] = [];
  filteredBooks: Book[] = [];
  searchQuery = '';
  displayedColumns = ['title', 'author', 'isbn', 'available', 'actions'];
  loading = false;

  constructor(
    private bookService: BookService,
    private borrowService: BorrowService,
    public authService: AuthService
  ) { }

  ngOnInit() {
    this.loading = true;
    this.bookService.getBooks().subscribe({
      next: (response) => {
        this.books = response.books;
        this.filteredBooks = this.books;
        this.loading = false;
      },
      error: () => (this.loading = false),
    });
  }

  filterBooks() {
    this.filteredBooks = this.books.filter(
      (book) =>
        book.title.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        book.author.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
  }

  borrowBook(bookId: string) {
    const userId = this.authService.getUser()?.id;
    if (userId) {
      this.loading = true;
      this.borrowService.issueBook(userId, bookId).subscribe({
        next: () => {
          this.ngOnInit();
          this.loading = false;
        },
        error: () => (this.loading = false),
      });
    }
  }
}