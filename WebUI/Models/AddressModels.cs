namespace WebUI.Models
{
    public class AddressDto
    {
        public int AddressID { get; set; }
        public int UserID { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public string ReceiverPhone { get; set; } = string.Empty;
        public string AddressDetail { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? District { get; set; }
        public string Ward { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        
        // GHN Integration Fields
        public int? GhnProvinceId { get; set; }
        public int? GhnDistrictId { get; set; }
        public string? GhnWardCode { get; set; }
        
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Computed full address for display
        public string FullAddress => $"{AddressDetail}, {Ward}, {District}, {City}";
    }
    
    public class CreateAddressReq
    {
        public int UserID { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public string ReceiverPhone { get; set; } = string.Empty;
        public string AddressDetail { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? District { get; set; }
        public string Ward { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        
        // GHN Integration Fields
        public int? GhnProvinceId { get; set; }
        public int? GhnDistrictId { get; set; }
        public string? GhnWardCode { get; set; }
        
        public bool IsDefault { get; set; }
    }
    
    public class UpdateAddressReq
    {
        public int AddressID { get; set; }
        public int UserID { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public string ReceiverPhone { get; set; } = string.Empty;
        public string AddressDetail { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? District { get; set; }
        public string Ward { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        
        // GHN Integration Fields
        public int? GhnProvinceId { get; set; }
        public int? GhnDistrictId { get; set; }
        public string? GhnWardCode { get; set; }
        
        public bool IsDefault { get; set; }
    }
}
