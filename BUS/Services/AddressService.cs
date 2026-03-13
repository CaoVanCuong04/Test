using DAL.DTOs.Address.Req;
using DAL.DTOs.Address.Res;
using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BUS.Services
{
    public class AddressService
    {
        private readonly AddressRepository _addressRepository;
        
        public AddressService(AddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }
        
        public async Task<List<AddressDto>> GetUserAddressesAsync(int userId)
        {
            var addresses = await _addressRepository.GetByUserIdAsync(userId);
            return addresses.Select(MapToDto).ToList();
        }
        
        public async Task<AddressDto?> GetAddressByIdAsync(int addressId, int userId)
        {
            var address = await _addressRepository.GetByIdAsync(addressId);
            
            // Verify ownership
            if (address == null || address.UserID != userId)
            {
                return null;
            }
            
            return MapToDto(address);
        }
        
        public async Task<AddressDto?> GetDefaultAddressAsync(int userId)
        {
            var address = await _addressRepository.GetDefaultAddressAsync(userId);
            return address != null ? MapToDto(address) : null;
        }
        
        public async Task<(bool Success, string Message, int AddressId)> CreateAddressAsync(CreateAddressReq request)
        {
            try
            {
                var address = new Address
                {
                    UserID = request.UserID,
                    ReceiverName = request.ReceiverName,
                    ReceiverPhone = request.ReceiverPhone,
                    AddressDetail = request.AddressDetail,
                    City = request.City,
                    District = request.District,
                    Ward = request.Ward,
                    Street = request.Street ?? string.Empty,
                    GhnProvinceId = request.GhnProvinceId,
                    GhnDistrictId = request.GhnDistrictId,
                    GhnWardCode = request.GhnWardCode,
                    IsDefault = request.IsDefault,
                    CreatedAt = DateTime.Now
                };
                
                var addressId = await _addressRepository.AddAsync(address);
                return (true, "Thêm địa chỉ thành công", addressId);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}", 0);
            }
        }
        
        public async Task<(bool Success, string Message)> UpdateAddressAsync(UpdateAddressReq request)
        {
            try
            {
                var address = await _addressRepository.GetByIdAsync(request.AddressID);
                
                if (address == null)
                {
                    return (false, "Không tìm thấy địa chỉ");
                }
                
                // Verify ownership
                if (address.UserID != request.UserID)
                {
                    return (false, "Bạn không có quyền sửa địa chỉ này");
                }
                
                address.ReceiverName = request.ReceiverName;
                address.ReceiverPhone = request.ReceiverPhone;
                address.AddressDetail = request.AddressDetail;
                address.City = request.City;
                address.District = request.District;
                address.Ward = request.Ward;
                address.Street = request.Street ?? string.Empty;
                address.GhnProvinceId = request.GhnProvinceId;
                address.GhnDistrictId = request.GhnDistrictId;
                address.GhnWardCode = request.GhnWardCode;
                address.IsDefault = request.IsDefault;
                
                await _addressRepository.UpdateAsync(address);
                return (true, "Cập nhật địa chỉ thành công");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }
        
        public async Task<(bool Success, string Message)> DeleteAddressAsync(int addressId, int userId)
        {
            try
            {
                var address = await _addressRepository.GetByIdAsync(addressId);
                
                if (address == null)
                {
                    return (false, "Không tìm thấy địa chỉ");
                }
                
                // Verify ownership
                if (address.UserID != userId)
                {
                    return (false, "Bạn không có quyền xóa địa chỉ này");
                }
                
                await _addressRepository.DeleteAsync(addressId);
                return (true, "Xóa địa chỉ thành công");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }
        
        public async Task<(bool Success, string Message)> SetDefaultAddressAsync(int addressId, int userId)
        {
            try
            {
                var address = await _addressRepository.GetByIdAsync(addressId);
                
                if (address == null)
                {
                    return (false, "Không tìm thấy địa chỉ");
                }
                
                // Verify ownership
                if (address.UserID != userId)
                {
                    return (false, "Bạn không có quyền thay đổi địa chỉ này");
                }
                
                await _addressRepository.SetDefaultAsync(addressId, userId);
                return (true, "Đặt địa chỉ mặc định thành công");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }
        
        private AddressDto MapToDto(Address address)
        {
            return new AddressDto
            {
                AddressID = address.AddressID,
                UserID = address.UserID,
                ReceiverName = address.ReceiverName,
                ReceiverPhone = address.ReceiverPhone,
                AddressDetail = address.AddressDetail,
                City = address.City,
                District = address.District,
                Ward = address.Ward,
                Street = address.Street,
                GhnProvinceId = address.GhnProvinceId,
                GhnDistrictId = address.GhnDistrictId,
                GhnWardCode = address.GhnWardCode,
                IsDefault = address.IsDefault,
                CreatedAt = address.CreatedAt,
                UpdatedAt = address.UpdatedAt
            };
        }
    }
}
