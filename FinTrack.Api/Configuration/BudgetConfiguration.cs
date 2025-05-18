using FinTrack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Api.Configuration;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.PlannedAmountOfMoney)
            .HasColumnType("decimal(18,4)");

        builder.Property(x => x.TotalAmountOfMoney)
            .HasColumnType("decimal(18,4)");
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Budgets)
            .HasForeignKey(x => x.UserId);

        builder
            .HasOne(x => x.Currency)
            .WithMany(x => x.Budgets)
            .HasForeignKey(x => x.CurrencyId);

        builder
            .HasMany(x => x.Expences)
            .WithOne(x => x.Budget);

        builder
            .HasMany(x => x.Incomes)
            .WithOne(x => x.Budget);
    }
}
