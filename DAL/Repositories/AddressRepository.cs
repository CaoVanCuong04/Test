using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class AddressRepository
    {
        private readonly AppDbContext _context;
        public AddressRepository(AppDbContext context)
        {
            this._context = context;
        }
        
        public async Task<List<Address>> GetAllAsync()
        {
            return await _context.Addresses.ToListAsync();
        }
        
        public async Task<List<Address>> GetByUserIdAsync(int userId)
        {
            return await _context.Addresses
                .Where(a => a.UserID == userId)
                .OrderByDescending(a => a.IsDefault)
                .ThenByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
        
        public async Task<Address?> GetByIdAsync(int id)
        {
            return await _context.Addresses.FindAsync(id);
        }
        
        public async Task<Address?> GetDefaultAddressAsync(int userId)
        {
            return await _context.Addresses
                .Where(a => a.UserID == userId && a.IsDefault)
                .FirstOrDefaultAsync();
        }
        
        public async Task<int> AddAsync(Address address)
        {
            // If this is set as default, unset all other defaults for this user
            if (address.IsDefault)
            {
                await UnsetAllDefaultsAsync(address.UserID);
            }
            
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address.AddressID;
        }
        
        public async Task UpdateAsync(Address address)
        {
            // If this is set as default, unset all other defaults for this user
            if (address.IsDefault)
            {
                await UnsetAllDefaultsAsync(address.UserID, address.AddressID);
            }
            
            address.UpdatedAt = DateTime.Now;
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(int id)
        {
            var address = await GetByIdAsync(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task SetDefaultAsync(int addressId, int userId)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressID == addressId && a.UserID == userId);
            
            if (address != null)
            {
                // Unset all other defaults
                await UnsetAllDefaultsAsync(userId, addressId);
                
                // Set this as default
                address.IsDefault = true;
                address.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
        
        private async Task UnsetAllDefaultsAsync(int userId, int? excludeAddressId = null)
        {
            var defaultAddresses = await _context.Addresses
                .Where(a => a.UserID == userId && a.IsDefault)
                .ToListAsync();
            
            if (excludeAddressId.HasValue)
            {
                defaultAddresses = defaultAddresses
                    .Where(a => a.AddressID != excludeAddressId.Value)
                    .ToList();
            }
            
            foreach (var addr in defaultAddresses)
            {
                addr.IsDefault = false;
                addr.UpdatedAt = DateTime.Now;
            }
            
            if (defaultAddresses.Any())
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}
