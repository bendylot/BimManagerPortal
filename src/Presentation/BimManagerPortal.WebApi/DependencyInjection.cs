using System.Text.Json;
using BimManagerPortal.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;

namespace BimManagerPortal.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // Controllers
        services.AddControllers();
        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        // Swagger / OpenAPI
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "PluginsBigData API", Version = "v1" });

            c.MapType<JsonElement>(() => new OpenApiSchema
            {
                Type = JsonSchemaType.Object
            });
        });

        return services;
    }

    public static WebApplication ConfigurePresentation(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            try
            {
                db.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
                logger.LogCritical(ex, "Database migration failed on startup.");
                throw;
            }
        }

        app.UseRouting();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "PluginsBigDataManager v1");
            c.RoutePrefix = "swagger";
        });

        app.UseHttpsRedirection();

        // CORS
        app.UseCors("AllowAll");

        // Controllers
        app.MapControllers();

        return app;
    }
}