using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using BuddyBlazor.Models;

namespace BuddyBlazor.Client
{
    public class AppAuthClient
    {
        private HttpClient _httpClient;
        public AppAuthClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<object> RegisterGoogleUser(HandleGoogleUserModel googleUserRequestModel)
        {
            var postData = new StringContent(JsonSerializer.Serialize(googleUserRequestModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/user/HandleGoogleUser", postData);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<object>();
        }
    }
}
