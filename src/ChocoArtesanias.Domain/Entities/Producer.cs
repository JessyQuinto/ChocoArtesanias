namespace ChocoArtesanias.Domain.Entities;

public class Producer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool Featured { get; set; }
    public int FoundationYear { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactAddress { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
