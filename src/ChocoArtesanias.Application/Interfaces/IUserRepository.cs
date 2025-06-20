using ChocoArtesanias.Domain.Entities;

namespace ChocoArtesanias.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByIdWithAddressesAsync(Guid id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    
    // TODO: Address methods commented out since Address is now an owned entity
    // These methods should be removed or refactored if standalone address management is needed
    /*
    Task<Address?> GetAddressByIdAsync(Guid addressId, Guid userId);
    Task AddAddressAsync(Address address);
    Task UpdateAddressAsync(Address address);
    Task DeleteAddressAsync(Guid addressId, Guid userId);
    */
}
