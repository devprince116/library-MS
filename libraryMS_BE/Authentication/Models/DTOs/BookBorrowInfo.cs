using Authentication.Models.Entities;

namespace Authentication.Models.DTOs;

public class BookBorrowInfo
{
    public Books Book { get; set; }
    public int BorrowCount { get; set; }
}