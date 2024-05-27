using BuddyBlazor.Models;
using Flurl;
using Flurl.Http;

namespace BuddyBlazor.Client
{
    public class AppAuthClient
    {
        private const string AuthBaseUrl = "https://localhost:44373";

        public async Task<GoogleAuthResult> RegisterGoogleUser(HandleGoogleUserModel googleUserRequestModel)
        {
            var result = await AuthBaseUrl.AppendPathSegments("api", "user", "HandleGoogleUser")
                .PostJsonAsync(googleUserRequestModel)
                .ReceiveJson<GoogleAuthResult>();  

            return result;
        }
    }
}
