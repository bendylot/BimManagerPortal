using BimManagerPortal.WebAssembly;
using BimManagerPortal.WebAssembly.Components.ModalForm.JsonWatcher;
using BimManagerPortal.WebAssembly.Services.PluginReports;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7067") });
builder.Services.AddScoped<IPluginReportProviderServiceProvider, PluginReportProviderServiceProvider>();
builder.Services.AddScoped<JsonWatcherModalService>();

await builder.Build().RunAsync();