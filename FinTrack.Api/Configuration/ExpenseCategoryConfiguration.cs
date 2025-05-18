using FinTrack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Api.Configuration;

public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
{
    public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .HasMany(x => x.Expenses)
            .WithOne(x => x.ExpenseCategory);
    }
}
