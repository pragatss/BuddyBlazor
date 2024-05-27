using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BuddyBlazor;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

Dictionary<string, object> settings = builder.Configuration
    .GetSection("Local")
    .Get<Dictionary<string, object>>();

builder.Services.AddOidcAuthentication(options => {
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

await builder.Build().RunAsync();

