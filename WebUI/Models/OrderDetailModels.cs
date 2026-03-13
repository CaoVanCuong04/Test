using System.Text.Json.Serialization;

namespace WebUI.Models
{
    public class GetOrderDetailRes
    {
        [JsonPropertyName("orderID")]
        public int OrderID { get; set; }

        [JsonPropertyName("orderCode")]
        public string OrderCode { get; set; } = string.Empty;

        [JsonPropertyName("userName")]
        public string UserName { get; set; } = string.Empty;

        [JsonPropertyName("fullName")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [JsonPropertyName("paymentMethod")]
        public string PaymentMethod { get; set; } = string.Empty;

        [JsonPropertyName("paymentStatus")]
        public int PaymentStatus { get; set; }

        [JsonPropertyName("paymentStatusText")]
        public string PaymentStatusText { get; set; } = string.Empty;

        [JsonPropertyName("shippingProvider")]
        public string? ShippingProvider { get; set; }

        [JsonPropertyName("trackingNumber")]
        public string? TrackingNumber { get; set; }

        [JsonPropertyName("shippedDate")]
        public DateTime? ShippedDate { get; set; }

        [JsonPropertyName("voucherCode")]
        public string VoucherCode { get; set; } = string.Empty;

        [JsonPropertyName("orderType")]
        public string OrderType { get; set; } = string.Empty;

        [JsonPropertyName("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("subtotal")]
        public decimal Subtotal { get; set; }

        [JsonPropertyName("shippingFee")]
        public decimal ShippingFee { get; set; }

        [JsonPropertyName("discount")]
        public decimal Discount { get; set; }

        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("ghnFullAddress")]
        public string? GhnFullAddress { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; } = string.Empty;

        [JsonPropertyName("listProduct")]
        public List<GetProductDetailRes> ListProduct { get; set; } = new();
    }

    public class GetProductDetailRes
    {
        [JsonPropertyName("productID")]
        public int ProductID { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;

        [JsonPropertyName("gendersName")]
        public string GendersName { get; set; } = string.Empty;

        [JsonPropertyName("brandName")]
        public string BrandName { get; set; } = string.Empty;

        [JsonPropertyName("importPrice")]
        public decimal ImportPrice { get; set; }

        [JsonPropertyName("sellingPrice")]
        public decimal SellingPrice { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;

        public decimal TotalPrice => SellingPrice * Quantity;
    }
}
