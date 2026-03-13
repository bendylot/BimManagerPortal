using System.Text.Json;
using BimManagerPortal.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace BimManagerPortal.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // Controllers
        services.AddControllers();
        services.AddOpenApi();
        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("http://37.230.113.96") // IP вашего сервера (где висит фронт)
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
            db.Database.Migrate();
        }
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        
        // Swagger
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "PluginsBigDataManager v1");
            c.RoutePrefix = "swagger";
        });

        // CORS
        app.UseCors("AllowAll");

        // Routing
        app.UseRouting();

        // Controllers
        app.MapControllers();

        return app;
    }
}