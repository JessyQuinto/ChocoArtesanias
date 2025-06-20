using ChocoArtesanias.Application.Interfaces;
using ChocoArtesanias.Domain.Entities;
using ChocoArtesanias.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChocoArtesanias.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }
    
    public async Task<User?> GetByIdWithAddressesAsync(Guid id)
    {
        // Note: Addresses are now owned entities in Orders, not standalone
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        user.UpdatedAt = DateTime.UtcNow;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();    }
    
    // TODO: Address methods commented out since Address is now an owned entity
    // These methods should be removed or refactored if standalone address management is needed
    /*
    public async Task<Address?> GetAddressByIdAsync(Guid addressId, Guid userId)
    {
        return await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);
    }

    public async Task AddAddressAsync(Address address)
    {
        // If this is the first address, make it default
        var hasAddresses = await _context.Addresses
            .AnyAsync(a => a.UserId == address.UserId);
            
        if (!hasAddresses)
        {
            address.IsDefault = true;
        }
        
        // If making this default, remove default from others
        if (address.IsDefault)
        {
            var existingAddresses = await _context.Addresses
                .Where(a => a.UserId == address.UserId && a.IsDefault)
                .ToListAsync();
                
            foreach (var addr in existingAddresses)
            {
                addr.IsDefault = false;
            }
        }

        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAddressAsync(Address address)
    {
        // If making this default, remove default from others
        if (address.IsDefault)
        {
            var existingAddresses = await _context.Addresses
                .Where(a => a.UserId == address.UserId && a.IsDefault && a.Id != address.Id)
                .ToListAsync();
                
            foreach (var addr in existingAddresses)
            {
                addr.IsDefault = false;
            }
        }
        
        _context.Addresses.Update(address);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAddressAsync(Guid addressId, Guid userId)
    {
        var address = await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);
            
        if (address != null)
        {
            _context.Addresses.Remove(address);
            
            // If this was the default address, make another one default
            if (address.IsDefault)
            {
                var nextAddress = await _context.Addresses
                    .Where(a => a.UserId == userId)
                    .FirstOrDefaultAsync();
                    
                if (nextAddress != null)
                {
                    nextAddress.IsDefault = true;
                }
            }
            
            await _context.SaveChangesAsync();
        }
    }
    */
}
