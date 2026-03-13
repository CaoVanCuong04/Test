using Microsoft.JSInterop;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;

namespace AdminWeb.Services
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly NavigationManager _navigationManager;

        public AuthorizationMessageHandler(IJSRuntime jsRuntime, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                // Lấy token từ localStorage
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "admin_access_token");

                if (!string.IsNullOrEmpty(token))
                {
                    // Thêm token vào Authorization header
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (Exception ex)
            {
                // Log error if needed
                Console.WriteLine($"Error getting token: {ex.Message}");
            }

            var response = await base.SendAsync(request, cancellationToken);

            // ✅ Kiểm tra 401 Unauthorized
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                try
                {
                    // Xóa tokens
                    await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "admin_access_token");
                    await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "admin_refresh_token");
                    await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "admin_user_id");
                    
                    // Redirect to login
                    _navigationManager.NavigateTo("/login", forceLoad: true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error handling 401: {ex.Message}");
                }
            }

            return response;
        }
    }
}
