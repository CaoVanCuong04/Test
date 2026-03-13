using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using WebUI.Models;

namespace WebUI.Services
{
    public class AddressService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ConfigurationService _configService;

        public AddressService(IHttpClientFactory httpClientFactory, ConfigurationService configService)
        {
            _httpClientFactory = httpClientFactory;
            _configService = configService;
        }

        public async Task<List<AddressDto>> GetUserAddressesAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("AuthorizedAPI");
                var apiBaseUrl = await _configService.GetApiBaseUrlAsync();
                var response = await httpClient.GetAsync($"{apiBaseUrl}/api/Address");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AddressApiResponse>();
                    return result?.Data ?? new List<AddressDto>();
                }

                return new List<AddressDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AddressService] Error getting addresses: {ex.Message}");
                return new List<AddressDto>();
            }
        }

        public async Task<AddressDto?> GetDefaultAddressAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("AuthorizedAPI");
                var apiBaseUrl = await _configService.GetApiBaseUrlAsync();
                var response = await httpClient.GetAsync($"{apiBaseUrl}/api/Address/default");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<SingleAddressApiResponse>();
                    return result?.Data;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AddressService] Error getting default address: {ex.Message}");
                return null;
            }
        }

        public async Task<AddressDto?> GetAddressByIdAsync(int addressId)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("AuthorizedAPI");
                var apiBaseUrl = await _configService.GetApiBaseUrlAsync();
                var response = await httpClient.GetAsync($"{apiBaseUrl}/api/Address/{addressId}");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<SingleAddressApiResponse>();
                    return result?.Data;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AddressService] Error getting address: {ex.Message}");
                return null;
            }
        }

        public async Task<(bool Success, string Message, int AddressId)> CreateAddressAsync(CreateAddressReq request)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("AuthorizedAPI");
                var apiBaseUrl = await _configService.GetApiBaseUrlAsync();
                var response = await httpClient.PostAsJsonAsync($"{apiBaseUrl}/api/Address", request);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<CreateAddressApiResponse>();
                    return (result?.Success ?? false, result?.Message ?? "Lỗi không xác định", result?.AddressId ?? 0);
                }

                // Try to read error as text first for debugging
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[AddressService] API Error Response ({response.StatusCode}): {errorContent}");
                
                try
                {
                    var errorResult = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                    return (false, errorResult?.Message ?? $"Lỗi {response.StatusCode}: {errorContent}", 0);
                }
                catch
                {
                    return (false, $"Lỗi {response.StatusCode}: {errorContent}", 0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AddressService] Error creating address: {ex.Message}");
                Console.WriteLine($"[AddressService] Stack trace: {ex.StackTrace}");
                return (false, $"Lỗi: {ex.Message}", 0);
            }
        }

        public async Task<(bool Success, string Message)> UpdateAddressAsync(int addressId, UpdateAddressReq request)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("AuthorizedAPI");
                var apiBaseUrl = await _configService.GetApiBaseUrlAsync();
                var response = await httpClient.PutAsJsonAsync($"{apiBaseUrl}/api/Address/{addressId}", request);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    return (result?.Success ?? false, result?.Message ?? "Lỗi không xác định");
                }

                var errorResult = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                return (false, errorResult?.Message ?? "Lỗi khi cập nhật địa chỉ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AddressService] Error updating address: {ex.Message}");
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteAddressAsync(int addressId)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("AuthorizedAPI");
                var apiBaseUrl = await _configService.GetApiBaseUrlAsync();
                var response = await httpClient.DeleteAsync($"{apiBaseUrl}/api/Address/{addressId}");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    return (result?.Success ?? false, result?.Message ?? "Lỗi không xác định");
                }

                var errorResult = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                return (false, errorResult?.Message ?? "Lỗi khi xóa địa chỉ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AddressService] Error deleting address: {ex.Message}");
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> SetDefaultAddressAsync(int addressId)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("AuthorizedAPI");
                var apiBaseUrl = await _configService.GetApiBaseUrlAsync();
                var response = await httpClient.PutAsync($"{apiBaseUrl}/api/Address/{addressId}/set-default", null);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    return (result?.Success ?? false, result?.Message ?? "Lỗi không xác định");
                }

                var errorResult = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                return (false, errorResult?.Message ?? "Lỗi khi đặt địa chỉ mặc định");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AddressService] Error setting default address: {ex.Message}");
                return (false, $"Lỗi: {ex.Message}");
            }
        }
    }

    // API Response Models
    public class AddressApiResponse
    {
        public bool Success { get; set; }
        public List<AddressDto> Data { get; set; } = new();
    }

    public class SingleAddressApiResponse
    {
        public bool Success { get; set; }
        public AddressDto? Data { get; set; }
    }

    public class CreateAddressApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int AddressId { get; set; }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class ApiErrorResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
