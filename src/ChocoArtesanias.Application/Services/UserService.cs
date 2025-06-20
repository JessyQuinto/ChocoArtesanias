using ChocoArtesanias.Application.DTOs;
using ChocoArtesanias.Application.Interfaces;
using ChocoArtesanias.Domain.Entities;

namespace ChocoArtesanias.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }    public async Task<UserProfileResponse?> GetProfileAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdWithAddressesAsync(userId);
        if (user == null) return null;

        // Addresses are now owned entities in Orders, not standalone
        var addresses = new List<AddressResponse>(); // Empty list for now

        return new UserProfileResponse(
            user.Id.ToString(),
            user.FirstName,
            user.LastName,
            user.Email,
            user.Phone,
            addresses,
            user.CreatedAt,
            user.UpdatedAt
        );
    }

    public async Task<UserProfileResponse?> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return null;

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Phone = request.Phone;

        await _userRepository.UpdateAsync(user);        return await GetProfileAsync(userId);
    }

    // TODO: Address management methods commented out since Address is now an owned entity
    // These methods should be removed or refactored if standalone address management is needed
    /*
    public async Task<AddressResponse> AddAddressAsync(Guid userId, AddressRequest request)
    {
        var address = new Address
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            FullName = request.FullName,
            StreetAddress = request.Address,
            City = request.City,
            PostalCode = request.PostalCode,
            Phone = request.Phone,
            IsDefault = request.IsDefault,
            UserId = userId
        };

        await _userRepository.AddAddressAsync(address);

        return new AddressResponse(
            address.Id.ToString(),
            address.Name,
            address.FullName,
            address.StreetAddress,
            address.City,
            address.PostalCode,
            address.Phone,
            address.IsDefault
        );
    }

    public async Task<AddressResponse?> UpdateAddressAsync(Guid userId, Guid addressId, AddressRequest request)
    {
        var address = await _userRepository.GetAddressByIdAsync(addressId, userId);
        if (address == null) return null;

        address.Name = request.Name;
        address.FullName = request.FullName;
        address.StreetAddress = request.Address;
        address.City = request.City;
        address.PostalCode = request.PostalCode;
        address.Phone = request.Phone;
        address.IsDefault = request.IsDefault;

        await _userRepository.UpdateAddressAsync(address);

        return new AddressResponse(
            address.Id.ToString(),
            address.Name,
            address.FullName,
            address.StreetAddress,
            address.City,
            address.PostalCode,
            address.Phone,
            address.IsDefault
        );
    }    public async Task<bool> DeleteAddressAsync(Guid userId, Guid addressId)
    {
        var address = await _userRepository.GetAddressByIdAsync(addressId, userId);
        if (address == null) return false;

        await _userRepository.DeleteAddressAsync(addressId, userId);
        return true;
    }
    */
}
