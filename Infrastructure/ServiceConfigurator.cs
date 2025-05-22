using Domain.Common;
using Domain.Deals;
using Domain.Orders;
using Domain.Wallets;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class InfrastructureOptions {
    public string DatabaseConnectionString { get; init; } = string.Empty;
}

public static class ServiceConfigurator {
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        Action<InfrastructureOptions> configure) {
        var options = new InfrastructureOptions();
        configure(options);

        services.AddRepositories(options.DatabaseConnectionString);
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services, string connectionString) {
        services.AddDbContext<ApplicationDbContext>(
            optionsBuilder => optionsBuilder.UseNpgsql(
                connectionString,
                npgOptions => npgOptions.EnableRetryOnFailure()),
            contextLifetime: ServiceLifetime.Scoped,
            optionsLifetime: ServiceLifetime.Singleton);

        services.AddScoped<IDealsRepository, DealsRepository>();
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IWalletsRepository, WalletsRepository>();

        return services;
    }
}
