using System.Security.Claims;
using Authentication.Models.Entities;
using Authentication.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BorrowController : ControllerBase
{
    private readonly IBorrowService _borrowService;

    public BorrowController(IBorrowService borrowService)
    {
        _borrowService = borrowService;
    }

    [HttpPost("issue")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> IssueBook([FromQuery] Guid userId, [FromQuery] Guid bookId)
    {
        try
        {
            var borrowRecord = await _borrowService.IssueBookAsync(userId, bookId);
            return Ok(new { message = "Book issued successfully", BorrowRecord = borrowRecord });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("return")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> ReturnBook([FromQuery] Guid borrowRecordId)
    {
        try
        {
            await _borrowService.ReturnBookAsync(borrowRecordId);
            return Ok(new { message = "Book returned successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllBorrowRecords()
    {
        try
        {
            var records = await _borrowService.GetAllBorrowRecordsAsync();
            return Ok(new { message = "All borrow records fetched", Records = records });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("user/{userId}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetUserBorrowRecords(Guid userId)
    {
        try
        {
          // ensure user can see their records only;
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (currentUserIdClaim == null)
            {
                return Unauthorized(new { message = "User identity not found in token" });
            }

            if (!Guid.TryParse(currentUserIdClaim.Value, out Guid currentUserId))
            {
                return BadRequest(new { message = "Invalid user ID in token" });
            }
            
            if (User.IsInRole("User") && currentUserId != userId)
            {
                return Unauthorized(new { message = "Use can see their record only" });
            }

            var records = await _borrowService.GetBorrowRecordsByUserAsync(userId);
            return Ok(new { message = "User borrow records fetched", Records = records });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}