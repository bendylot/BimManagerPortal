using BimManagerPortal.WebAssembly;
using BimManagerPortal.WebAssembly.Auth;
using BimManagerPortal.WebAssembly.Components.ModalForm.JsonWatcher;
using BimManagerPortal.WebAssembly.Components.ModalForm.Loading;
using BimManagerPortal.WebAssembly.Services.ErrorDictionary;
using BimManagerPortal.WebAssembly.Services.PluginConfigurations;
using BimManagerPortal.WebAssembly.Services.PluginReports;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });

builder.Services.AddHttpClient<IPluginConfigurationService, PluginConfigurationService>(client =>
    client.BaseAddress = baseAddress);

builder.Services.AddScoped<IPluginReportProviderServiceProvider, PluginReportProviderServiceProvider>();
builder.Services.AddScoped<IErrorDictionaryService, ErrorDictionaryService>();
builder.Services.AddScoped<JsonWatcherModalService>();
builder.Services.AddScoped<LoadingModalService>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    sp => sp.GetRequiredService<CustomAuthStateProvider>());

await builder.Build().RunAsync();