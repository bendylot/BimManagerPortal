using BimManagerPortal.Application.Interfaces.ApiServices;
using BimManagerPortal.Application.Interfaces.Compress;
using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.BigDataPlugins;
using BimManagerPortal.Persistance.ApiServices;
using BimManagerPortal.Persistance.Contexts;
using BimManagerPortal.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BimManagerPortal.Persistance;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("Default"));
        });
        services.AddHttpClient();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);

            // Детальные ошибки только в Development
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == Environments.Development)
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });


        services.AddScoped<IExternalApiService, ExternalApiService>();
        services.AddScoped<ICompressionService, GzipCompressionService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGenericRepository<PluginBigData>, GenericRepository<PluginBigData>>();
        return services;
    }
}