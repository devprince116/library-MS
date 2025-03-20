using Authentication.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("book-count")]
    [Authorize(Roles = "Admin,User")] 
    public async Task<IActionResult> GetBookCount()
    {
        try
        {
            var count = await _analyticsService.GetBookCountAsync();
            return Ok(new { message = "Book count fetched", Count = count });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("most-borrowed")]
    [Authorize(Roles = "Admin,User")] 
    public async Task<IActionResult> GetMostBorrowedBook()
    {
        try
        {
            var (book, count) = await _analyticsService.GetMostBorrowedBookAsync();
            return Ok(new { message = "Most borrowed book fetched", Book = book, BorrowCount = count });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("top-10-borrowed")]
    [Authorize(Roles = "Admin,User")] 
    public async Task<IActionResult> GetTop10BorrowedBooks()
    {
        try
        {
            var topBooks = await _analyticsService.GetTop10BorrowedBooksAsync();
            return Ok(new { message = "Top 10 borrowed books fetched", TopBooks = topBooks });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}