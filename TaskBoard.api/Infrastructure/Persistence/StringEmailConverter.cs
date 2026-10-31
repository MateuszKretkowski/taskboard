using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Infrastructure.Persistence;

public class StringEmailConverter : ValueConverter<Email, string>
{
    public StringEmailConverter() : base(
        toDatabase => toDatabase.Value,
        fromDatabase => new Email(fromDatabase)
    )
    {
        
    }
}