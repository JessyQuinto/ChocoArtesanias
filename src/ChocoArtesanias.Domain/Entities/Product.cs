namespace ChocoArtesanias.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? DiscountedPrice { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public List<string> Images { get; set; } = [];
    public int Stock { get; set; }
    public bool Featured { get; set; }
    public double Rating { get; set; }
    public string Artisan { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    public Guid ProducerId { get; set; }
    public Producer? Producer { get; set; }
    
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
