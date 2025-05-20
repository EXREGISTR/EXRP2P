namespace API.Endpoints.Users;

public static class UsersEndpoints {
    public static void MapUsersEndpoints(this IEndpointRouteBuilder builder) {
        builder.MapPost("/users/register", () => { });
    }
}
