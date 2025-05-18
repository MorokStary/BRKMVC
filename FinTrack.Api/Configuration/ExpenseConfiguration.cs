using FinTrack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Api.Configuration;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.ExpenseVolume)
            .HasColumnType("decimal(18,4)");

        builder
            .HasOne(x => x.Budget)
            .WithMany(x => x.Expences)
            .HasForeignKey(x => x.BudgetId);
    }
}
