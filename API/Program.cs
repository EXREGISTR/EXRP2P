using API.Endpoints.Currencies;
using API.Endpoints.Orders;
using API.Endpoints.Users;
using API.Endpoints.Wallets;

namespace API;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }

        app.UseRouting();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        var group = app.MapGroup("/api");

        group.MapCurrenciesEndpoints();
        group.MapUsersEndpoints();
        group.MapWalletsEndpoints();
        group.MapOrdersEndpoints();

        app.Run();
    }
}
