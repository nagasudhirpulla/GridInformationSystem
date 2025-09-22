namespace DataApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddHealthChecks();
        services.AddEndpointsApiExplorer();
        services.AddControllers();
        services.AddOpenApi("Data API");
        return services;
    }
}