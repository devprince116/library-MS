using Authentication.Models.Entities;
using Authentication.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Services;

public class BorrowService : IBorrowService
{
    private readonly AppDbContext _context;

    public BorrowService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BorrowRecord> IssueBookAsync(Guid userId, Guid bookId)
    {
     
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new Exception("User not found");


        var book = await _context.Books.FindAsync(bookId);
        if (book == null)
            throw new Exception("Book not found");
        if (book.AvailableCopies <= 0)
            throw new Exception("No available copies of the book");

        var borrowRecord = new BorrowRecord
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            BookId = bookId,
            BorrowDate = DateTime.UtcNow,
            ReturnDate = null
        };

   
        book.AvailableCopies--;
        _context.BorrowRecords.Add(borrowRecord);
        await _context.SaveChangesAsync();


        var result = await _context.BorrowRecords
            .Include(br => br.User)  
            .Include(br => br.Book)  
            .FirstOrDefaultAsync(br => br.Id == borrowRecord.Id);

        return result ?? throw new Exception("Failed to retrieve borrow record");
    }

    public async Task ReturnBookAsync(Guid borrowRecordId)
    {
        var borrowRecord = await _context.BorrowRecords
            .Include(br => br.Book)
            .FirstOrDefaultAsync(br => br.Id == borrowRecordId);
        if (borrowRecord == null)
            throw new Exception("Borrow record not found");
        if (borrowRecord.ReturnDate.HasValue)
            throw new Exception("Book already returned");

        borrowRecord.ReturnDate = DateTime.UtcNow;
        borrowRecord.Book.AvailableCopies++;
        await _context.SaveChangesAsync();
    }

    public async Task<List<BorrowRecord>> GetAllBorrowRecordsAsync()
    {
        return await _context.BorrowRecords
            .Include(br => br.User)  
            .Include(br => br.Book)
            .ToListAsync();
    }

    public async Task<List<BorrowRecord>> GetBorrowRecordsByUserAsync(Guid userId)
    {
        return await _context.BorrowRecords
            .Where(br => br.UserId == userId)
            .Include(br => br.User)  
            .Include(br => br.Book)
            .ToListAsync();
    }
}