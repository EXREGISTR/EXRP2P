using Domain.Common;

namespace Infrastructure.Services;

internal sealed class DateTimeProvider : IDateTimeProvider {
    public DateTime UtcNow => DateTime.UtcNow;
}
