using System.Security.Claims;

namespace API.Utils;

public class UserContextAccessor(IHttpContextAccessor httpContextAccessor) {
    public Guid Id => Guid.Parse(
        httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
}
