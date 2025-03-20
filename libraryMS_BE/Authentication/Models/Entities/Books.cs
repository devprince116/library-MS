namespace Authentication.Models.Entities;

public class Books
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Isbn { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
}