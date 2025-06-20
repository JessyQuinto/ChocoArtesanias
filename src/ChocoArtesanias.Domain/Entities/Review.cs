namespace ChocoArtesanias.Domain.Entities;

public class Review
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public int Rating { get; set; } // 1 to 5
    public string Title { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
