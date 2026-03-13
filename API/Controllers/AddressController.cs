using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using BUS.Services;
using DAL.DTOs.Address.Req;
using DAL.DTOs.Address.Res;
using Helper.Utils;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly AddressService _addressService;

        public AddressController(AddressService addressService)
        {
            _addressService = addressService;
        }

        /// <summary>
        /// Get all addresses for logged-in user
        /// </summary>
        [HttpGet]
        [BAuthorize]
        public async Task<IActionResult> GetUserAddresses()
        {
            var userId = HttpContextHelper.GetUserId();
            var addresses = await _addressService.GetUserAddressesAsync(userId);
            return Ok(new { success = true, data = addresses });
        }

        /// <summary>
        /// Get default address for logged-in user
        /// </summary>
        [HttpGet("default")]
        [BAuthorize]
        public async Task<IActionResult> GetDefaultAddress()
        {
            var userId = HttpContextHelper.GetUserId();
            var address = await _addressService.GetDefaultAddressAsync(userId);
            if (address == null)
            {
                return NotFound(new { success = false, message = "Không có địa chỉ mặc định" });
            }

            return Ok(new { success = true, data = address });
        }

        /// <summary>
        /// Get address by ID
        /// </summary>
        [HttpGet("{id}")]
        [BAuthorize]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var userId = HttpContextHelper.GetUserId();
            var address = await _addressService.GetAddressByIdAsync(id, userId);
            if (address == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy địa chỉ" });
            }

            return Ok(new { success = true, data = address });
        }

        /// <summary>
        /// Create new address
        /// </summary>
        [HttpPost]
        [BAuthorize]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressReq request)
        {
            var userId = HttpContextHelper.GetUserId();
            request.UserID = userId;

            var (success, message, addressId) = await _addressService.CreateAddressAsync(request);
            
            if (success)
            {
                return Ok(new { success = true, message, addressId });
            }

            return BadRequest(new { success = false, message });
        }

        /// <summary>
        /// Update existing address
        /// </summary>
        [HttpPut("{id}")]
        [BAuthorize]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] UpdateAddressReq request)
        {
            var userId = HttpContextHelper.GetUserId();
            request.AddressID = id;
            request.UserID = userId;

            var (success, message) = await _addressService.UpdateAddressAsync(request);
            
            if (success)
            {
                return Ok(new { success = true, message });
            }

            return BadRequest(new { success = false, message });
        }

        /// <summary>
        /// Delete address
        /// </summary>
        [HttpDelete("{id}")]
        [BAuthorize]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var userId = HttpContextHelper.GetUserId();
            var (success, message) = await _addressService.DeleteAddressAsync(id, userId);
            
            if (success)
            {
                return Ok(new { success = true, message });
            }

            return BadRequest(new { success = false, message });
        }

        /// <summary>
        /// Set address as default
        /// </summary>
        [HttpPut("{id}/set-default")]
        [BAuthorize]
        public async Task<IActionResult> SetDefaultAddress(int id)
        {
            var userId = HttpContextHelper.GetUserId();
            var (success, message) = await _addressService.SetDefaultAddressAsync(id, userId);
            
            if (success)
            {
                return Ok(new { success = true, message });
            }

            return BadRequest(new { success = false, message });
        }
    }
}
