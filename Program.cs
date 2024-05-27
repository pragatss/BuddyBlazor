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

builder.Services.AddMudServices();

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

