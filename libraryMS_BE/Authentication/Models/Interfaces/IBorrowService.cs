using Authentication.Models.Entities;

namespace Authentication.Models.Interfaces;

public interface IBorrowService
{
    Task<BorrowRecord> IssueBookAsync(Guid userId, Guid bookId);
    Task ReturnBookAsync(Guid borrowRecordId);
    Task<List<BorrowRecord>> GetAllBorrowRecordsAsync();
    Task<List<BorrowRecord>> GetBorrowRecordsByUserAsync(Guid userId);
}