import { Book } from "./book.model";
import { User } from "./user.model";

export interface BorrowRecord {
    id: string;
    userId: string;
    bookId: string;
    borrowDate: string;
    returnDate?: string;
    user: User;
    book: Book;
}