using Authentication.Models.Entities;

namespace Authentication.Models.Interfaces;

public interface IBookService
{
    Task ImportBooksAsync(IFormFile file);
    Task<Books> AddBookAsync(Books book);
    Task<List<Books>> GetAllBooksAsync();
    Task<Books> GetBookByIdAsync(Guid id);
    Task UpdateBookAsync(Guid id, Books book);
    Task DeleteBookAsync(Guid id);
}