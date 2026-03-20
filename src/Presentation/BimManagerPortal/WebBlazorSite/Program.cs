using BimManagerPortal.Application.Interfaces.ApiServices;
using BimManagerPortal.Persistance.ApiServices;
using BimManagerPortal.WebBlazorSite.UIComponents;
using BimManagerPortal.WebBlazorSite.UIComponents.UiServices.ModalForm.JsonWatcher;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient<IExternalApiService, ExternalApiService>(client =>
{
    // Убедитесь, что адрес заканчивается на слеш /
    client.BaseAddress = new Uri("http://37.230.113.96:5001/");
});
builder.Services.AddScoped<JsonWatcherModalService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
