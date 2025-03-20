// src/app/components/admin-dashboard/admin-dashboard.component.ts
import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { BookService } from '../../../services/book.service';
import { AnalyticsService } from '../../../services/analytics.service';
import { BorrowService } from '../../../services/borrow.service';
import { Router } from '@angular/router';
import { fadeInOut } from '../../../assets/animations';
import { LoaderComponent } from '../../loader/loader.component';
import { Book } from '../../../models/book.model';
import { BorrowRecord } from '../../../models/borrow.model';
import Chart from 'chart.js/auto';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatTableModule,
    ReactiveFormsModule,
    LoaderComponent,
  ],
  templateUrl: "./admin-dashboard.component.html",
  styleUrl: "./admin-dashboard.component.scss",
  animations: [fadeInOut],
})
export class AdminDashboardComponent implements OnInit, AfterViewInit {
  @ViewChild('fileInput') fileInput: any;
  @ViewChild('chartCanvas') chartCanvas!: ElementRef<HTMLCanvasElement>;

  bookForm!: FormGroup;
  bookCount = 0;
  mostBorrowed: { book: Book; borrowCount: number } | null = null;
  overdueRecords: BorrowRecord[] = [];
  displayedColumns = ['user', 'title', 'borrowDate'];
  loading = false;
  chart!: Chart;

  constructor(
    private fb: FormBuilder,
    private bookService: BookService,
    private analyticsService: AnalyticsService,
    private borrowService: BorrowService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.bookForm = this.fb.group({
      title: ['', Validators.required],
      author: ['', Validators.required],
      isbn: ['', Validators.required],
      totalCopies: [0, Validators.required],
      availableCopies: [0, Validators.required],
    });
  }

  ngOnInit() {
    this.loading = true;
    this.analyticsService.getBookCount().subscribe({
      next: (response) => {
        this.bookCount = response.count;
        this.updateChart();
      },
      error: (err) => console.error('Error fetching book count:', err),
    });
    this.analyticsService.getMostBorrowedBook().subscribe({
      next: (response) => {
        this.mostBorrowed = response;
        this.updateChart();
      },
      error: (err) => console.error('Error fetching most borrowed:', err),
    });
    this.borrowService.getAllBorrowRecords().subscribe({
      next: (response) => {
        this.overdueRecords = response.records.filter(
          (record) => !record.returnDate && new Date(record.borrowDate) < new Date(Date.now() - 14 * 24 * 60 * 60 * 1000) // 14 days overdue
        );
      },
      error: (err) => console.error('Error fetching borrow records:', err),
      complete: () => (this.loading = false),
    });
  }

  ngAfterViewInit() {
    this.createChart();
  }

  createChart() {
    const ctx = this.chartCanvas.nativeElement.getContext('2d');
    if (ctx) {
      this.chart = new Chart(ctx, {
        type: 'bar',
        data: {
          labels: ['Total Books', 'Most Borrowed'],
          datasets: [
            {
              label: 'Library Analytics',
              data: [this.bookCount, this.mostBorrowed?.borrowCount || 0],
              backgroundColor: ['#26a69a', '#ff7043'],
              borderColor: ['#00695c', '#f4511e'],
              borderWidth: 1,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          scales: {
            y: {
              beginAtZero: true,
            },
          },
        },
      });
    }
  }

  updateChart() {
    if (this.chart) {
      this.chart.data.datasets[0].data = [this.bookCount, this.mostBorrowed?.borrowCount || 0];
      this.chart.update();
    }
  }

  addBook() {
    if (this.bookForm.valid) {
      this.loading = true;
      this.bookService.addBook(this.bookForm.value).subscribe({
        next: () => {
          this.loading = false;
          this.snackBar.open('Book added successfully!', 'Close', {
            duration: 3000,
            horizontalPosition: 'center',
            verticalPosition: 'top',
          });
          this.bookForm.reset();
          this.router.navigate(['/']);
        },
        error: () => (this.loading = false),
      });
    }
  }

  importBooks(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      this.loading = true;
      this.bookService.importBooks(file).subscribe({
        next: () => {
          this.ngOnInit();
          this.snackBar.open('Books imported successfully!', 'Close', {
            duration: 3000,
            horizontalPosition: 'center',
            verticalPosition: 'top',
          });
          this.loading = false;
        },
        error: () => (this.loading = false),
      });
    }
  }
}