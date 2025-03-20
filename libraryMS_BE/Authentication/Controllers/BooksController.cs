using Authentication.Models.DTOs;
using Authentication.Models.Entities;
using Authentication.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost("import")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ImportBooks(IFormFile file)
    {
        try
        {
            await _bookService.ImportBooksAsync(file);
            return Ok(new { message = "Books imported successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddBook([FromBody] AddBooksDto request)
    {
        try
        {
            var book = new Books()
            {
                Title = request.Title,
                Author = request.Author,
                Isbn = request.Isbn,
                TotalCopies = request.TotalCopies,
                AvailableCopies = request.AvailableCopies
                
            };
            var addedBook = await _bookService.AddBookAsync(book);
            return Ok(new { message = "Book added successfully", Book = addedBook });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    // [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetBooks()
    {
        try
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(new { message = "Books fetched successfully", Books = books });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetBook(Guid id)
    {
        try
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound(new { message = "Book not found" });
            return Ok(new { message = "Book fetched successfully", Book = book });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateBook(Guid id, [FromBody] AddBooksDto request)
    {
        try
        {
            var book = new Books()
            {
                Id = id, 
                Title = request.Title,
                Author = request.Author,
                Isbn = request.Isbn,
                TotalCopies = request.TotalCopies,
                AvailableCopies = request.AvailableCopies
            };
            await _bookService.UpdateBookAsync(id, book);
            return Ok(new { message = "Book updated successfully" , book });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        try
        {
            await _bookService.DeleteBookAsync(id);
            return Ok(new { message = "Book deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}