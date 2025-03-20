import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { BorrowService } from '../../../services/borrow.service';
import { AuthService } from '../../../services/auth.service';
import { BorrowRecord } from '../../../models/borrow.model';
import { slideInOut } from '../../../assets/animations';
import { LoaderComponent } from "../../loader/loader.component";
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-user-dashboard',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, LoaderComponent],
  templateUrl: "./user-dashboard.component.html",
  styleUrl: "./user-dashboard.component.scss",
  animations: [slideInOut],
})
export class UserDashboardComponent implements OnInit {
  borrowRecords: BorrowRecord[] = [];
  displayedColumns = ['title', 'borrowDate', 'returnDate', 'actions'];
  loading = false;

  constructor(
    private borrowService: BorrowService,
    private authService: AuthService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit() {
    const userId = this.authService.getUser()?.id;
    if (userId) {
      this.loading = true;
      this.borrowService.getUserBorrowRecords(userId).subscribe({
        next: (response) => {
          this.borrowRecords = response.records;
          this.loading = false;
        },
        error: () => (this.loading = false),
      });
    }
  }

  returnBook(borrowRecordId: string) {
    this.loading = true;
    this.borrowService.returnBook(borrowRecordId).subscribe({
      next: () => {
        this.snackBar.open('Book returned successfully!', 'Close', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'top',
        });
        this.ngOnInit();
        this.loading = false;
      },
      error: () => (this.loading = false),
    });
  }
}