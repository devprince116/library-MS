import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { BorrowService } from '../../services/borrow.service';
import { BorrowRecord } from '../../models/borrow.model';
import { slideInOut } from '../../assets/animations';
import { LoaderComponent } from "../loader/loader.component";

@Component({
  selector: 'app-overdue-reports',
  standalone: true,
  imports: [CommonModule, MatTableModule, LoaderComponent],
  templateUrl: "./due-report.component.html",
  styles: [
    `
      .reports-container {
        padding: 20px;
      }
      mat-table {
        width: 100%;
      }
      h2 {
        margin-bottom: 20px;
      }
    `,
  ],
  animations: [slideInOut],
})
export class OverdueReportsComponent implements OnInit {
  overdueRecords: BorrowRecord[] = [];
  displayedColumns = ['user', 'title', 'borrowDate'];
  loading = false;

  constructor(private borrowService: BorrowService) { }

  ngOnInit() {
    this.loading = true;
    this.borrowService.getAllBorrowRecords().subscribe({
      next: (response) => {
        this.overdueRecords = response.records.filter(
          (record) => !record.returnDate && new Date(record.borrowDate) < new Date(Date.now() - 14 * 24 * 60 * 60 * 1000) // 14 days due time
        );
        this.loading = false;
      },
      error: () => (this.loading = false),
    });
  }
}