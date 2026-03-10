using System.Reflection;
using BimManagerPortal.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace BimManagerPortal.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR - регистрация всех Handlers, Behaviors
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        
        return services;
    }
}