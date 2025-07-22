using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Nudge.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddScoped<ApiService>();

await builder.Build().RunAsync();