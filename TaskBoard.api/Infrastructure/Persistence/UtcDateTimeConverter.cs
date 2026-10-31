using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TaskBoard.Api.Infrastructure.Persistence;

public class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeConverter() : base(
        toDatabase => toDatabase,
        fromDatabase => DateTime.SpecifyKind(fromDatabase, DateTimeKind.Utc)
    )
    {
        
    }
}