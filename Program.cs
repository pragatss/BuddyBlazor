using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BuddyBlazor;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using BuddyBlazor.Providers;
using BuddyBlazor.Client;
using Blazored.SessionStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

Dictionary<string, object> settings = builder.Configuration
    .GetSection("Local")
    .Get<Dictionary<string, object>>();


builder.Services.AddHttpClient<AppAuthClient>(options => {
    options.BaseAddress = new Uri("https://localhost:44373");
});

builder.Services.AddBlazoredSessionStorage();

builder.Services.AddOidcAuthentication<RemoteAuthenticationState,
RemoteUserAccount>(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);

    options.ProviderOptions.DefaultScopes.Add("email");
})
.AddAccountClaimsPrincipalFactory<RemoteAuthenticationState,
RemoteUserAccount, CustomAccountFactory>();

await builder.Build().RunAsync();

