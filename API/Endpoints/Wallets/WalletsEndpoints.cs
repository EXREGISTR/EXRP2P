namespace API.Endpoints.Wallets;

public static class WalletsEndpoints {

    public static void MapWalletsEndpoints(this IEndpointRouteBuilder builder) {
        builder.MapPost("/register", () => { });
    }
}
