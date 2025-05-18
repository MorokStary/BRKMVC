using FinTrack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Api.Configuration;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .HasMany(x => x.Budgets)
            .WithOne(x => x.Currency);
    }
}
