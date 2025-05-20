namespace API.Endpoints.Currencies;

public static class CurrenciesEndpoints {
    public static void MapCurrenciesEndpoints(this IEndpointRouteBuilder builder) {
        builder.MapPost("/currencies/register", () => { });
    }
}