namespace Authentication.Models.Entities;

public class BorrowRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public User? User { get; set; }
    public Books? Book { get; set; }
}