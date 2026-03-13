using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace WebUI.Services
{
    /// <summary>
    /// Service để tính phí ship và xử lý các tác vụ liên quan đến vận chuyển
    /// </summary>
    public class ShippingService
    {
        private readonly HttpClient _httpClient;
        private readonly ConfigurationService _configService;

        public ShippingService(HttpClient httpClient, ConfigurationService configService)
        {
            _httpClient = httpClient;
            _configService = configService;
        }

        /// <summary>
        /// Tính phí vận chuyển dựa trên địa chỉ và thông tin đơn hàng
        /// </summary>
        /// <param name="request">Thông tin yêu cầu tính phí</param>
        /// <returns>Phí ship (VND) hoặc null nếu có lỗi</returns>
        public async Task<decimal?> CalculateShippingFeeAsync(ShippingFeeRequest request)
        {
            try
            {
                var apiBaseUrl = await _configService.GetApiBaseUrlAsync();
                
                // Tạo payload với snake_case để GHN API nhận đúng
                var payload = new
                {
                    service_type_id = request.ServiceTypeId,
                    from_district_id = request.FromDistrictId,
                    to_district_id = request.ToDistrictId,
                    to_ward_code = request.ToWardCode,
                    weight = request.Weight,
                    length = request.Length,
                    width = request.Width,
                    height = request.Height,
                    insurance_value = request.InsuranceValue,
                    items = request.Items?.Select(i => new
                    {
                        name = i.Name,
                        quantity = i.Quantity,
                        price = i.Price
                    }).ToList()
                };

                var response = await _httpClient.PostAsJsonAsync($"{apiBaseUrl}/api/shipping/calculate-fee", payload);
                
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ShippingService] Response status: {response.StatusCode}");
                Console.WriteLine($"[ShippingService] Response body: {responseContent}");
                
                if (response.IsSuccessStatusCode)
                {
                    // API Controller trả về trực tiếp GhnCalculateFeeResponse, không wrap trong GhnApiResponse
                    var result = System.Text.Json.JsonSerializer.Deserialize<GhnFeeData>(
                        responseContent,
                        new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                    
                    Console.WriteLine($"[ShippingService] ✅ Parsed total: {result?.Total}đ (service: {result?.ServiceFee}đ, insurance: {result?.InsuranceFee}đ)");
                    
                    // GHN trả về số nguyên (VND), convert sang decimal
                    return result?.Total != null ? (decimal)result.Total : null;
                }
                
                Console.WriteLine($"[ShippingService] ❌ Calculate fee error: {response.StatusCode} - {responseContent}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ShippingService] Exception calculating fee: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Tính phí cho cả ship thường và ship hỏa tốc cùng lúc
        /// </summary>
        public async Task<ShippingFeeResult> CalculateBothShippingFeesAsync(
            int toDistrictId,
            string toWardCode,
            int totalWeight,
            int length,
            int width,
            int height,
            decimal subtotal,
            List<ShippingFeeItem>? items = null)
        {
            var result = new ShippingFeeResult();

            try
            {
                // Tính phí ship thường (ServiceTypeId = 2)
                var standardRequest = new ShippingFeeRequest
                {
                    ServiceTypeId = 2,
                    FromDistrictId = 1542, // District 1, HCMC (kho ASION)
                    ToDistrictId = toDistrictId,
                    ToWardCode = toWardCode,
                    Weight = totalWeight,
                    Length = length,
                    Width = width,
                    Height = height,
                    InsuranceValue = (int)subtotal,
                    Items = items
                };

                Console.WriteLine($"[ShippingService] 📦 Calling Standard shipping (ServiceTypeId=2) with {items?.Count ?? 0} items");
                result.StandardFee = await CalculateShippingFeeAsync(standardRequest) ?? 15000m;

                // Tính phí Express: Tạm thời dùng giá ước tính (thực tế cần gọi API get-service trước)
                // Express thường cao hơn Standard khoảng 1.5-2 lần
                result.ExpressFee = result.StandardFee * 1.5m;
                result.Success = true;

                Console.WriteLine($"[ShippingService] Calculated fees - Standard: {result.StandardFee:N0}đ, Express: {result.ExpressFee:N0}đ (estimated)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ShippingService] Error calculating both fees: {ex.Message}");
                // Fallback values
                result.StandardFee = 15000m;
                result.ExpressFee = 25000m;
                result.Success = false;
            }

            return result;
        }

        /// <summary>
        /// Ước tính kích thước gói hàng dựa trên số lượng sản phẩm
        /// </summary>
        public static (int length, int width, int height) EstimatePackageDimensions(int quantity)
        {
            // Kích thước cơ bản cho 1 đôi giày
            int baseLength = 30;
            int baseWidth = 20;
            int baseHeight = 15;

            // Tăng kích thước theo số lượng
            int length = baseLength + (quantity > 2 ? (quantity - 2) * 2 : 0);
            int width = baseWidth + (quantity > 1 ? quantity * 3 : 0);
            int height = baseHeight + (quantity > 1 ? quantity * 2 : 0);

            return (length, width, height);
        }

        /// <summary>
        /// Ước tính trọng lượng dựa trên số lượng sản phẩm
        /// </summary>
        public static int EstimatePackageWeight(int quantity)
        {
            // Mỗi đôi giày khoảng 400-500g
            return quantity * 400;
        }
    }

    /// <summary>
    /// Request để tính phí ship (internal use)
    /// </summary>
    public class ShippingFeeRequest
    {
        public int ServiceTypeId { get; set; } // 2 = Standard, 5 = Express
        public int FromDistrictId { get; set; }
        public int ToDistrictId { get; set; }
        public string ToWardCode { get; set; } = string.Empty;
        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int InsuranceValue { get; set; }
        public List<ShippingFeeItem>? Items { get; set; }
    }

    /// <summary>
    /// Item trong đơn hàng để tính phí
    /// </summary>
    public class ShippingFeeItem
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int Price { get; set; }
    }

    /// <summary>
    /// Kết quả tính phí cho cả 2 loại ship
    /// </summary>
    public class ShippingFeeResult
    {
        public decimal StandardFee { get; set; }
        public decimal ExpressFee { get; set; }
        public bool Success { get; set; }
    }

    /// <summary>
    /// Response từ GHN API
    /// </summary>
    public class GhnFeeResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public GhnFeeData? Data { get; set; }
    }

    public class GhnFeeData
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("service_fee")]
        public int ServiceFee { get; set; }

        [JsonPropertyName("insurance_fee")]
        public int? InsuranceFee { get; set; }
    }
}
