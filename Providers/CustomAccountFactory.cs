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
using Microsoft.JSInterop;

namespace BuddyBlazor.Providers
{
    public class CustomAccountFactory
    : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        private readonly AppAuthClient _appAuthClient;
        private readonly IConfiguration _config;
        private readonly IJSRuntime _js;
        private readonly ISessionStorageService _sessionStorageService;
        public CustomAccountFactory(IAccessTokenProviderAccessor accessor,
            AppAuthClient appAuthClient,
            IConfiguration config,
            IJSRuntime js,
            ISessionStorageService service
            ) : base(accessor)
        {
            _appAuthClient = appAuthClient;
            _config = config;
            _js = js;
            _sessionStorageService = service;

        }

        private async Task<string> ReadIdToken()
        {
            var clientId = _config.GetValue<string>("Google:ClientId");
            var authority = _config.GetValue<string>("Local:Authority");
            var userDataKey = $"oidc.user:{authority}:{clientId}";
            var userData = await _sessionStorageService.GetItemAsync<UserData>(userDataKey);
            return userData.id_token;
        }

        class UserData
        {
            public string id_token { get; set; }
            public int expires_at { get; set; }
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(
            RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var initialUser = await base.CreateUserAsync(account, options);
            try
            {
                if (initialUser.Identity.IsAuthenticated)
                {
                    var googleUser = new HandleGoogleUserModel
                    {
                        Email = initialUser.Claims.Where(_ => _.Type == "email").Select(_ => _.Value).FirstOrDefault(),
                        GoogleToken = await ReadIdToken()
                        //,
                        //FirstName = initialUser.Claims.Where(_ => _.Type == "given_name").Select(_ => _.Value).FirstOrDefault(),

                        //LastName = initialUser.Claims.Where(_ => _.Type == "family_name").Select(_ => _.Value).FirstOrDefault()
                    };

                    var response = await _appAuthClient.RegisterGoogleUser(googleUser);

                    //((ClaimsIdentity)initialUser.Identity).AddClaim(
                    //    new Claim("APIjwt", response.JwtToken)
                    //);

                }
            }
            catch
            {
                initialUser = new ClaimsPrincipal(new ClaimsIdentity());
            }

            return initialUser;
        }
    }
}

