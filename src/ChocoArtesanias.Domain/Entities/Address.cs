namespace ChocoArtesanias.Domain.Entities;

public class Address
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string StreetAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    // UserId and User navigation removed since Address is now an owned entity
    // public Guid UserId { get; set; }
    // public User? User { get; set; }
}
