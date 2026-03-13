using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs.Address.Req
{
    public class UpdateAddressReq
    {
        [Required(ErrorMessage = "AddressID is required")]
        public int AddressID { get; set; }
        
        [Required(ErrorMessage = "UserID is required")]
        public int UserID { get; set; }
        
        [Required(ErrorMessage = "Tên người nhận không được để trống")]
        [MaxLength(150)]
        public string ReceiverName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [MaxLength(20)]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string ReceiverPhone { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Địa chỉ chi tiết không được để trống")]
        [MaxLength(250)]
        public string AddressDetail { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Tỉnh/Thành phố không được để trống")]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? District { get; set; }
        
        [Required(ErrorMessage = "Phường/Xã không được để trống")]
        [MaxLength(100)]
        public string Ward { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string Street { get; set; } = string.Empty;
        
        // GHN Integration Fields
        public int? GhnProvinceId { get; set; }
        public int? GhnDistrictId { get; set; }
        [MaxLength(20)]
        public string? GhnWardCode { get; set; }
        
        public bool IsDefault { get; set; } = false;
    }
}
