using Carter;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        // Add API services and configurations here
        //services.AddCarter();
        
        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        // Use API services and configurations here
        //app.MapCarter();

        return app;
    }
}
