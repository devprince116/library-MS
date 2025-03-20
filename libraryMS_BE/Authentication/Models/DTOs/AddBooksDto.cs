namespace Authentication.Models.DTOs;

public class AddBooksDto
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string  Isbn { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    
}