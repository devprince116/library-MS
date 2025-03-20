using Authentication.Models.Entities;
using Authentication.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using LicenseContext = System.ComponentModel.LicenseContext;

namespace Authentication.Services;

public class BookService: IBookService
{
    private readonly AppDbContext _context;

    public BookService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Books> AddBookAsync(Books book)
    {
        book.Id = Guid.NewGuid();
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<List<Books>> GetAllBooksAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<Books> GetBookByIdAsync(Guid id)
    {
        return await _context.Books.FindAsync(id);
    }

    public async Task UpdateBookAsync(Guid id, Books book)
    {
        var existingBook = await _context.Books.FindAsync(id);
        if (existingBook == null) throw new Exception("Book not found");
        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.Isbn = book.Isbn;
        existingBook.TotalCopies = book.TotalCopies;
        existingBook.AvailableCopies = book.AvailableCopies;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(Guid id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) throw new Exception("Book not found");
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }

    public async Task ImportBooksAsync(IFormFile file)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        
        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        using var package = new ExcelPackage(stream);
        var worksheet = package.Workbook.Worksheets[0];
        
        var books = new List<Books>();
        for (int row = 2; row <= worksheet.Dimension.Rows; row++)
        {
            books.Add(new Books()
            {
                Title = worksheet.Cells[row, 1].Value?.ToString() ?? "",
                Author = worksheet.Cells[row, 2].Value?.ToString() ?? "",
                Isbn = worksheet.Cells[row, 3].Value?.ToString() ?? "",
                TotalCopies = int.TryParse(worksheet.Cells[row, 4].Value?.ToString(), out int copies) ? copies : 0,
                AvailableCopies = int.TryParse(worksheet.Cells[row, 4].Value?.ToString(), out int avail) ? avail : 0
            });
        }

        await _context.Books.AddRangeAsync(books);
        await _context.SaveChangesAsync();
    }
    
}