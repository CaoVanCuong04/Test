using System.Net.Http.Headers;
using Microsoft.JSInterop;
using WebUI.Services.Interfaces;
using WebUI.Constants;

namespace WebUI.Services
{
    /// <summary>
    /// HTTP Message Handler để tự động thêm JWT token vào Authorization header
    /// </summary>
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private readonly IAuthService _authService;
        private readonly IJSRuntime _jsRuntime;

        public AuthorizationMessageHandler(IAuthService authService, IJSRuntime jsRuntime)
        {
            _authService = authService;
            _jsRuntime = jsRuntime;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Console.WriteLine($"[AuthorizationMessageHandler] SendAsync called for: {request.RequestUri}");
            
            // Lấy token từ AuthService trước
            var token = _authService.CurrentToken;
            Console.WriteLine($"[AuthorizationMessageHandler] Token from AuthService: {(string.IsNullOrEmpty(token) ? "NULL" : "EXISTS")}");

            // Nếu không có, thử lấy từ localStorage
            if (string.IsNullOrEmpty(token))
            {
                try
                {
                    // KHÔNG truyền cancellationToken vào InvokeAsync - nó không serialize được
                    token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", LocalStorageKeys.AuthToken);
                    Console.WriteLine($"[AuthorizationMessageHandler] Token from localStorage: {(string.IsNullOrEmpty(token) ? "NULL" : token?.Substring(0, Math.Min(20, token.Length)) + "...")}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AuthorizationMessageHandler] Error getting token from localStorage: {ex.Message}");
                }
            }

            if (!string.IsNullOrEmpty(token) && !request.Headers.Contains("Authorization"))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine($"[AuthorizationMessageHandler] ✓ Added Authorization header: Bearer {token.Substring(0, Math.Min(20, token.Length))}...");
            }
            else if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine($"[AuthorizationMessageHandler] ✗ No token available");
            }
            else
            {
                Console.WriteLine($"[AuthorizationMessageHandler] Authorization header already exists");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
