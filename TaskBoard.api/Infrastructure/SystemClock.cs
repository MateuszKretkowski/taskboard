using TaskBoard.Api.Services;

namespace TaskBoard.Api.Infrastructure;

public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}