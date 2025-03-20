using Authentication.Models.Entities;
using Authentication.Models.DTOs;
using Authentication.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly AppDbContext _context;

    public AnalyticsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetBookCountAsync()
    {
        return await _context.Books.CountAsync();
    }

    public async Task<(Books? Book, int Count)> GetMostBorrowedBookAsync()
    {
        var mostBorrowed = await _context.BorrowRecords
            .GroupBy(br => br.BookId)
            .Select(g => new { BookId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .FirstOrDefaultAsync();
        if (mostBorrowed == null) return (null, 0);
        
        var book = await _context.Books.FindAsync(mostBorrowed.BookId);
        return (book, mostBorrowed.Count);
    }

    public async Task<List<BookBorrowInfo>> GetTop10BorrowedBooksAsync()
    {
        return await _context.BorrowRecords
            .GroupBy(br => br.BookId)
            .Select(g => new { BookId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .Join(_context.Books,
                borrow => borrow.BookId,
                book => book.Id,
                (borrow, book) => new BookBorrowInfo 
                { 
                    Book = book, 
                    BorrowCount = borrow.Count 
                })
            .ToListAsync();
    }
}