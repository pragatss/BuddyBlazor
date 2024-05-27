using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using BuddyBlazor.Client;
using BuddyBlazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.Configuration;

namespace BuddyBlazor.Providers
{
    public class CustomAccountFactory
    : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        private readonly AppAuthClient _appAuthClient;
        private readonly IConfiguration _config;
        private readonly ISessionStorageService _sessionStorageService;
        public CustomAccountFactory(IAccessTokenProviderAccessor accessor,
            AppAuthClient appAuthClient,
            IConfiguration config,
            ISessionStorageService service
            ) : base(accessor)
        {
            _appAuthClient = appAuthClient;
            _config = config;
            _sessionStorageService = service;

        }

        private async Task<string> ReadIdToken()
        {
            var clientId = _config.GetValue<string>("Google:ClientId");
            var authority = _config.GetValue<string>("Local:Authority");
            var userDataKey = $"oidc.user:{authority}:{clientId}";
            var userData = await _sessionStorageService.GetItemAsync<GoogleUserData>(userDataKey);
            return userData.id_token;
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(
            RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            try
            {
                var initialUser = await base.CreateUserAsync(account, options);

                if (initialUser.Identity?.IsAuthenticated ?? false)
                {
                    var googleUser = new HandleGoogleUserModel
                    {
                        Email = initialUser?.Claims?.Where(_ => _.Type == GoogleClaim.Email).Select(_ => _.Value).FirstOrDefault()!,
                        GoogleToken = await ReadIdToken()
                    };

                    var response = await _appAuthClient.RegisterGoogleUser(googleUser);
                }
            }
            catch
            {
                throw;
            }

            return new ClaimsPrincipal(new ClaimsIdentity())!;
        }
    }

    public static class GoogleClaim
    {
        public static string Email = "email";
    }

    public class GoogleUserData
    {
        public string id_token { get; set; }
        public int expires_at { get; set; }
    }
}

