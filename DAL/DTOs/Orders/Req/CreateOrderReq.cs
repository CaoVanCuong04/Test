using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs.Orders.Req
{
    public class CreateOrderReq
    {
        [Required(ErrorMessage = "UserID is required")]
        public int UserID { get; set; }
        public int? PaymentID { get; set; }
        public int? VoucherID { get; set; }
        
        [Range(0, 10000000, ErrorMessage = "ShippingFee must be >= 0")]
        public decimal ShippingFee { get; set; } = 0;

        [Required(ErrorMessage = "OrderType is required")]
        [MaxLength(50)]
        public string OrderType { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(200)]
        public string? Note { get; set; }

        // Guest Order Fields - Thông tin người nhận
        [MaxLength(150)]
        public string? Name { get; set; }
        
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "OrderDetails is required")]
        public List<CreateOrderDetailsReq> OrderDetails { get; set; } = new();
        
        // GHN Address Fields
        public int? GhnProvinceId { get; set; }
        public int? GhnDistrictId { get; set; }
        [MaxLength(20)]
        public string? GhnWardCode { get; set; }
        [MaxLength(500)]
        public string? GhnFullAddress { get; set; }
        
        /// <summary>
        /// GHN Service Type (CẢ 2 ĐỀU CÓ PHÍ - tính theo GHN API):
        /// - null hoặc 2 = Ship thường (3-5 ngày) - PHÍ RẺ (~15-30k tùy địa chỉ)
        /// - 53320 = Ship hỏa tốc (1-2 ngày) - PHÍ CAO (~25-50k tùy địa chỉ)
        /// Phí tăng theo: khoảng cách + cân nặng + số lượng
        /// </summary>
        public int? GhnServiceTypeId { get; set; }
    }
}
