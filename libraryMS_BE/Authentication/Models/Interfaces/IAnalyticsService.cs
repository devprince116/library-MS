using Authentication.Models.Entities;
using Authentication.Models.DTOs;

namespace Authentication.Models.Interfaces;

public interface IAnalyticsService
{
    Task<int> GetBookCountAsync();
    Task<(Books? Book, int Count)> GetMostBorrowedBookAsync();
    Task<List<BookBorrowInfo>> GetTop10BorrowedBooksAsync();
}